namespace fitness.api.Features.Auth.Dtos;

public record LoginRequest(
    string Email,
    string Password
    );