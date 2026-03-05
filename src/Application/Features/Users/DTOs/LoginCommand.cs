using FluentResults;
using MediatR;

namespace Application.Features.Users.DTOs;

public sealed record LoginCommand(string Email, string Password)
    : IRequest<Result<AuthResponse>>;