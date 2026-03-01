using fitness.api.Data;
using Microsoft.AspNetCore.Authorization; 
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;    
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace fitness.api.Features.Users;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    
    public UsersController(AppDbContext db) => _db = db;

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<object>> Me()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile is null)
            return NotFound("Profile not found.");

        return Ok(new
        {
            userId,
            profile.DisplayName,
            profile.Units,
            profile.CreatedAtUtc
        });
    }
    
    [HttpGet("me-debug")]
    public IActionResult MeDebug()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            AuthType = User.Identity?.AuthenticationType,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}