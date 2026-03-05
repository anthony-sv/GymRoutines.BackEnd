using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IEquipmentRepository
{
    Task<Equipment?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Equipment?> GetByIdWithVariantsAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Equipment>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Equipment equipment, CancellationToken ct = default);
    void Remove(Equipment equipment);
}