namespace Application.Common;

public record TokenPair(string AccessToken, string RefreshToken, DateTimeOffset ExpiresAt);