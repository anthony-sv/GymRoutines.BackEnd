using Domain.Common;

namespace Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string? RefreshToken { get; private set; }
    public DateTimeOffset? RefreshTokenExpiresAt { get; private set; }

    private readonly List<TrainingPlan> _trainingPlans = [];
    public IReadOnlyCollection<TrainingPlan> TrainingPlans => _trainingPlans.AsReadOnly();

    private User() { }

    public static User Create(string email, string displayName, string passwordHash) =>
        new()
        {
            Email = email.ToLowerInvariant().Trim(),
            DisplayName = displayName.Trim(),
            PasswordHash = passwordHash
        };

    public void UpdateRefreshToken(string token, DateTimeOffset expiresAt)
    {
        RefreshToken = token;
        RefreshTokenExpiresAt = expiresAt;
        Touch();
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
        Touch();
    }

    public void UpdateProfile(string displayName)
    {
        DisplayName = displayName.Trim();
        Touch();
    }
}