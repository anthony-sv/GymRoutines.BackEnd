using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Abstract base for a block within a workout day.
/// Uses Table-Per-Hierarchy (TPH) via a Discriminator.
/// </summary>
public abstract class WorkoutBlock : BaseEntity
{
    public Guid WorkoutDayId { get; protected set; }
    public WorkoutDay WorkoutDay { get; protected set; } = default!;
    public BlockType BlockType { get; protected set; }
    public int Order { get; protected set; }
    public string? Notes { get; protected set; }

    protected WorkoutBlock() { }

    internal void SetOrder(int order) => Order = order;
}