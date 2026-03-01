namespace fitness.api.Features.Auth.Dtos;

public record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc
    );