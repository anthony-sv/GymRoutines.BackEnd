using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IExerciseRepository
{
    Task<Exercise?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Exercise>> GetAllAsync(CancellationToken ct = default);
    Task<(IReadOnlyList<Exercise> Items, int TotalCount)> GetPagedAsync(
        string? search, int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Exercise exercise, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    void Remove(Exercise exercise);
}