using Application.Common.Interfaces;
using Application.Features.Users.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Queries;

public sealed class GetCurrentUserQueryHandler(
    ICurrentUserService currentUser,
    IUserRepository users) : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result.Fail(DomainErrors.General.Unauthorized());

        var user = await users.GetByIdAsync(currentUser.UserId.Value, ct);
        if (user is null)
            return Result.Fail(DomainErrors.User.NotFound(currentUser.UserId.Value));

        return Result.Ok(new UserDto(user.Id, user.Email, user.DisplayName));
    }
}