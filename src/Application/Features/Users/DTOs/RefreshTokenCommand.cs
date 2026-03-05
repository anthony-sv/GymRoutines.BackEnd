using FluentResults;
using MediatR;

namespace Application.Features.Users.DTOs;

public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<Result<AuthResponse>>;