using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class TrainingPlanRepository(AppDbContext db) : ITrainingPlanRepository
{
    public Task<TrainingPlan?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.TrainingPlans.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<TrainingPlan?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct) =>
        db.TrainingPlans
            .Include(p => p.WeekTemplates)
                .ThenInclude(w => w.Days)
                    .ThenInclude(d => d.Blocks)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<(IReadOnlyList<TrainingPlan> Items, int TotalCount)> GetPagedAsync(
        Guid? ownerUserId, bool includePublic, int page, int pageSize, CancellationToken ct)
    {
        var query = db.TrainingPlans.AsQueryable();

        if (ownerUserId.HasValue && includePublic)
            query = query.Where(p => p.OwnerId == ownerUserId || p.IsPublic);
        else if (ownerUserId.HasValue)
            query = query.Where(p => p.OwnerId == ownerUserId);
        else
            query = query.Where(p => p.IsPublic);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items.AsReadOnly(), total);
    }

    public async Task AddAsync(TrainingPlan plan, CancellationToken ct) =>
        await db.TrainingPlans.AddAsync(plan, ct);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct) =>
        db.TrainingPlans.AnyAsync(p => p.Id == id, ct);

    public void Remove(TrainingPlan plan) => db.TrainingPlans.Remove(plan);
}
