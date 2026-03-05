using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITrainingPlanRepository
{
    Task<TrainingPlan?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TrainingPlan?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<TrainingPlan> Items, int TotalCount)> GetPagedAsync(
        Guid? ownerUserId,
        bool includePublic,
        int page,
        int pageSize,
        CancellationToken ct = default);
    Task AddAsync(TrainingPlan plan, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    void Remove(TrainingPlan plan);
}