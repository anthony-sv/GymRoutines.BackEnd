using Application.Common.Interfaces;
using Application.Features.Users.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands;

public sealed class RefreshTokenCommandHandler(
    IUserRepository users,
    ITokenService tokenService,
    IUnitOfWork uow) : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var userId = tokenService.ValidateRefreshToken(request.RefreshToken);
        if (userId is null)
            return Result.Fail(DomainErrors.User.InvalidCredentials());

        var user = await users.GetByIdAsync(userId.Value, ct);
        if (user is null || user.RefreshToken != request.RefreshToken
            || user.RefreshTokenExpiresAt <= DateTimeOffset.UtcNow)
            return Result.Fail(DomainErrors.User.InvalidCredentials());

        var tokens = tokenService.GenerateTokens(user);
        user.UpdateRefreshToken(tokens.RefreshToken, tokens.ExpiresAt);

        await uow.SaveChangesAsync(ct);

        return Result.Ok(new AuthResponse(
            user.Id, user.Email, user.DisplayName,
            tokens.AccessToken, tokens.RefreshToken, tokens.ExpiresAt));
    }
}