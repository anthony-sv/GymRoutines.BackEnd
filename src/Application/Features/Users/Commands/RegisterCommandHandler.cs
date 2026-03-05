using Application.Common.Interfaces;
using Application.Features.Users.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Domain.Entities;

namespace Application.Features.Users.Commands;

public sealed class RegisterCommandHandler(
    IUserRepository users,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IUnitOfWork uow) : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await users.ExistsByEmailAsync(request.Email, ct))
            return Result.Fail(DomainErrors.User.EmailAlreadyExists(request.Email));

        var hash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.Email, request.DisplayName, hash);
        await users.AddAsync(user, ct);

        var tokens = tokenService.GenerateTokens(user);
        user.UpdateRefreshToken(tokens.RefreshToken, tokens.ExpiresAt);

        await uow.SaveChangesAsync(ct);

        return Result.Ok(new AuthResponse(
            user.Id, user.Email, user.DisplayName,
            tokens.AccessToken, tokens.RefreshToken, tokens.ExpiresAt));
    }
}