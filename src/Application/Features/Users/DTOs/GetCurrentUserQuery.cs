using FluentResults;
using MediatR;

namespace Application.Features.Users.DTOs;

public sealed record GetCurrentUserQuery : IRequest<Result<UserDto>>;