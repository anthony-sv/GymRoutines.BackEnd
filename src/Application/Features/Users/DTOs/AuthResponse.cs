namespace Application.Features.Users.DTOs;

public sealed record AuthResponse(
    Guid UserId,
    string Email,
    string DisplayName,
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt);