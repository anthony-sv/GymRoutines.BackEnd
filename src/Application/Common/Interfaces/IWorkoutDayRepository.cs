using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IWorkoutDayRepository
{
    Task<WorkoutDay?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<WorkoutDay?> GetByIdWithBlocksAsync(Guid id, CancellationToken ct = default);
}