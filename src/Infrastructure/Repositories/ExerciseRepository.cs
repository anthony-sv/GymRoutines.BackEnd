using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class ExerciseRepository(AppDbContext db) : IExerciseRepository
{
    public Task<Exercise?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Exercises
            .Include(e => e.DefaultEquipment)
            .Include(e => e.DefaultEquipmentVariant)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<IReadOnlyList<Exercise>> GetAllAsync(CancellationToken ct) =>
        db.Exercises
            .Include(e => e.DefaultEquipment)
            .Include(e => e.DefaultEquipmentVariant)
            .OrderBy(e => e.Name)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Exercise>)t.Result.AsReadOnly(), ct);

    public async Task<(IReadOnlyList<Exercise> Items, int TotalCount)> GetPagedAsync(
        string? search, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Exercises
            .Include(e => e.DefaultEquipment)
            .Include(e => e.DefaultEquipmentVariant)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => EF.Functions.ILike(e.Name, $"%{search}%"));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items.AsReadOnly(), total);
    }

    public async Task AddAsync(Exercise exercise, CancellationToken ct) =>
        await db.Exercises.AddAsync(exercise, ct);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct) =>
        db.Exercises.AnyAsync(e => EF.Functions.ILike(e.Name, name), ct);

    public void Remove(Exercise exercise) => db.Exercises.Remove(exercise);
}
