namespace Application.Features.Users.DTOs;

public sealed record UserDto(Guid Id, string Email, string DisplayName);