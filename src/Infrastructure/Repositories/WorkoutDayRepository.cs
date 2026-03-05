using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class WorkoutDayRepository(AppDbContext db) : IWorkoutDayRepository
{
    public Task<WorkoutDay?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.WorkoutDays.FirstOrDefaultAsync(d => d.Id == id, ct);

    public Task<WorkoutDay?> GetByIdWithBlocksAsync(Guid id, CancellationToken ct) =>
        db.WorkoutDays
            .Include(d => d.WeekTemplate)
                .ThenInclude(w => w.TrainingPlan)
            .Include(d => d.Blocks)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
}
