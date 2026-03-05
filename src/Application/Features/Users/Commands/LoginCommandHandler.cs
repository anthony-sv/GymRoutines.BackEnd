using Application.Common.Interfaces;
using Application.Features.Users.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands;

public sealed class LoginCommandHandler(
    IUserRepository users,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IUnitOfWork uow) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await users.GetByEmailAsync(request.Email, ct);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Fail(DomainErrors.User.InvalidCredentials());

        var tokens = tokenService.GenerateTokens(user);
        user.UpdateRefreshToken(tokens.RefreshToken, tokens.ExpiresAt);

        await uow.SaveChangesAsync(ct);

        return Result.Ok(new AuthResponse(
            user.Id, user.Email, user.DisplayName,
            tokens.AccessToken, tokens.RefreshToken, tokens.ExpiresAt));
    }
}