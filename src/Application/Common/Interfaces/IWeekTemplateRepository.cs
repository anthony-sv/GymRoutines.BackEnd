using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IWeekTemplateRepository
{
    Task<WeekTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<WeekTemplate?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
}