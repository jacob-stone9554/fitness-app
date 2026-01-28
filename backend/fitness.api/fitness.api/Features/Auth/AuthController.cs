using fitness.api.Data;
using fitness.api.Data.Entities;
using fitness.api.Features.Auth.Dtos;
using fitness.api.Features.Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    
    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        AppDbContext db,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            return BadRequest("Email is already in use.");

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        var profile = new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            DisplayName = request.DisplayName,
            Units = "imperial",
            CreatedAtUtc = DateTime.UtcNow,
        };
        
        _db.UserProfiles.Add(profile);
        await _db.SaveChangesAsync();

        var (token, expiresAtUtc) = _tokenService.CreateToken(user);
        return Ok(new AuthResponse(token, expiresAtUtc));
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
            return Unauthorized("Invalid email or password.");

        var signIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!signIn.Succeeded)
            return Unauthorized("Invalid email or password.");

        var (token, expiresAtUtc) = _tokenService.CreateToken(user);
        return Ok(new AuthResponse(token, expiresAtUtc));
    }
}