using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    TokenPair GenerateTokens(User user);
    Guid? ValidateRefreshToken(string refreshToken);
}