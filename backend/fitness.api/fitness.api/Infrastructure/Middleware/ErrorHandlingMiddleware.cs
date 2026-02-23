using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
        
namespace fitness.api.Infrastructure.Errors;

public sealed class ErrorHandlingMiddleware : IMiddleware
{
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;
        
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
        {
                _logger = logger;
                _env = env;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
                try
                {
                        await next(context);
                }
                catch (Exception ex)
                {
                        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

                        var (status, title, detail, errors) = ex switch
                        {
                                AppValidationException ve => (StatusCodes.Status400BadRequest, "Validation Failed", ve.Message, ve.Errors),
                                NotFoundException nf     => (StatusCodes.Status404NotFound, "Not Found", nf.Message, null),
                                ConflictException cf     => (StatusCodes.Status409Conflict, "Conflict", cf.Message, null),
                                _                        => (StatusCodes.Status500InternalServerError, "Server Error",
                                        _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.", null),
                        };

                        if (status >= 500)
                                _logger.LogError(ex, "Unhandled exception. TraceId={TraceId}", traceId);
                        else
                                _logger.LogWarning(ex, "Request failed. TraceId={TraceId}", traceId);

                        var problem = new ProblemDetails
                        {
                                Status = status,
                                Title = title,
                                Detail = detail,
                                Instance = context.Request.Path
                        };

                        problem.Extensions["traceId"] = traceId;
                        if (errors is not null)
                                problem.Extensions["errors"] = errors;

                        context.Response.StatusCode = status;
                        context.Response.ContentType = "application/problem+json";

                        await context.Response.WriteAsJsonAsync(problem);
                }
        }
}