using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Variant of equipment — e.g. "Rope" or "V-Bar" for a cable machine.
/// </summary>
public sealed class EquipmentVariant : BaseEntity
{
    public Guid EquipmentId { get; private set; }
    public Equipment Equipment { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsSeeded { get; private set; }

    private EquipmentVariant() { }

    internal static EquipmentVariant Create(Guid equipmentId, string name, string? description = null, bool isSeeded = false) =>
        new()
        {
            EquipmentId = equipmentId,
            Name = name.Trim(),
            Description = description,
            IsSeeded = isSeeded
        };
}