using FluentResults;
using MediatR;

namespace Application.Features.Users.DTOs;

public sealed record RegisterCommand(string Email, string DisplayName, string Password)
    : IRequest<Result<AuthResponse>>;