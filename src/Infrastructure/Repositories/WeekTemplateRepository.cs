using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class WeekTemplateRepository(AppDbContext db) : IWeekTemplateRepository
{
    public Task<WeekTemplate?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.WeekTemplates.FirstOrDefaultAsync(w => w.Id == id, ct);

    public Task<WeekTemplate?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct) =>
        db.WeekTemplates
            .Include(w => w.TrainingPlan)
            .Include(w => w.Days)
            .FirstOrDefaultAsync(w => w.Id == id, ct);
}
