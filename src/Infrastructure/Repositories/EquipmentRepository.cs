using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class EquipmentRepository(AppDbContext db) : IEquipmentRepository
{
    public Task<Equipment?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Equipment.FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<Equipment?> GetByIdWithVariantsAsync(Guid id, CancellationToken ct) =>
        db.Equipment.Include(e => e.Variants).FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<IReadOnlyList<Equipment>> GetAllAsync(CancellationToken ct) =>
        db.Equipment
            .Include(e => e.Variants)
            .OrderBy(e => e.Name)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Equipment>)t.Result.AsReadOnly(), ct);

    public async Task AddAsync(Equipment equipment, CancellationToken ct) =>
        await db.Equipment.AddAsync(equipment, ct);

    public void Remove(Equipment equipment) => db.Equipment.Remove(equipment);
}