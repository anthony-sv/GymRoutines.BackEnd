using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Equipment : BaseEntity
{
    public string Name { get; private set; } = default!;
    public EquipmentCategory Category { get; private set; }
    public string? Description { get; private set; }
    public bool IsSeeded { get; private set; }

    private readonly List<EquipmentVariant> _variants = [];
    public IReadOnlyCollection<EquipmentVariant> Variants => _variants.AsReadOnly();

    private Equipment() { }

    public static Equipment Create(string name, EquipmentCategory category, string? description = null, bool isSeeded = false) =>
        new()
        {
            Name = name.Trim(),
            Category = category,
            Description = description,
            IsSeeded = isSeeded
        };

    public EquipmentVariant AddVariant(string name, string? description = null)
    {
        var variant = EquipmentVariant.Create(Id, name, description);
        _variants.Add(variant);
        Touch();
        return variant;
    }
}