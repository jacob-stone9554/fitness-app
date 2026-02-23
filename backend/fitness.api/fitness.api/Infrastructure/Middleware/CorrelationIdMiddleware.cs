using System.Diagnostics;
using Serilog.Context;

namespace fitness.api.Infrastructure.Middleware;

public sealed class CorrelationIdMiddleware : IMiddleware
{
    private const string HeaderName = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var incoming = context.Request.Headers[CorrelationIdMiddleware.HeaderName].FirstOrDefault();
        var correlationId = string.IsNullOrEmpty(incoming) ? Guid.NewGuid().ToString("n") : incoming;
        
        context.TraceIdentifier = correlationId;
        context.Response.Headers[CorrelationIdMiddleware.HeaderName] = correlationId;
        
        using(LogContext.PushProperty("TraceId", correlationId))
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
        
        // var incoming = context.Request.Headers[HeaderName].FirstOrDefault();
        // var correlationId = string.IsNullOrWhiteSpace(incoming) ? Guid.NewGuid().ToString("n") : incoming;
        //
        // context.TraceIdentifier = correlationId;
        // Activity.Current?.SetTag("correlation_id", correlationId);
        //
        // context.Response.Headers[HeaderName] = correlationId;
        //
        // await next(context);
    }
}