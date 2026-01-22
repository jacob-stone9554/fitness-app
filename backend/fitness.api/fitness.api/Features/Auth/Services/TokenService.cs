using fitness.api.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace fitness.api.Features.Auth.Services;

public interface ITokenService
{
    (string token, DateTime expiresAtUtc) CreateToken(AppUser user);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    
    public TokenService(IConfiguration config) => _config = config;

    public (string token, DateTime expiresAtUtc) CreateToken(AppUser user)
    {
        var issuer = _config["Jwt:Issuer"] ?? "fitness.api";
        var audience = _config["Jwt:Audience"] ?? "fitness.web";
        var key = _config["Jwt:Key"]
                  ?? throw new InvalidOperationException("Mising JWT: Key");

        var expires = DateTime.UtcNow.AddHours(2);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}