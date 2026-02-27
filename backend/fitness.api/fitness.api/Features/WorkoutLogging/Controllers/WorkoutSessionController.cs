using fitness.api.Data;
using fitness.api.Data.Entities;
using fitness.api.Features.Auth.Dtos;
using fitness.api.Features.Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace fitness.api.Features.WorkoutLogging.Controllers;

[ApiController]
[Route("workout")]
public class WorkoutSessionController : ControllerBase
{
    [HttpGet("CreateSession")]
    public IActionResult CreateSession()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            AuthType = User.Identity?.AuthenticationType,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}


