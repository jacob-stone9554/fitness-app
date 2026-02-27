using fitness.api.Data;
using fitness.api.Data.Entities;
using fitness.api.Features.Auth.Dtos;
using fitness.api.Infrastructure.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Features.Auth.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;

    public AuthService(
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

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new ConflictException("Email is already in use.");

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(
                e => e.Code,
                e => new[] { e.Description });
            throw new AppValidationException("Registration failed.", errors);
        }

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
        return new AuthResponse(token, expiresAtUtc);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
            return null;

        var signIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!signIn.Succeeded)
            return null;

        var (token, expiresAtUtc) = _tokenService.CreateToken(user);
        return new AuthResponse(token, expiresAtUtc);
    }
}