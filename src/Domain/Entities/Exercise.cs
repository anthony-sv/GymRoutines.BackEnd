using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Catalog entry for an exercise. Can be strength (rep-based),
/// timed (pose, hold, flex — duration-based), or cardio.
/// </summary>
public sealed class Exercise : BaseEntity
{
    public string Name { get; private set; } = default!;
    public ExerciseType Type { get; private set; }
    public string? Description { get; private set; }
    public bool IsSeeded { get; private set; }

    // Navigation to default equipment (optional — e.g. "Standing Barbell Curl" defaults to Barbell)
    public Guid? DefaultEquipmentId { get; private set; }
    public Equipment? DefaultEquipment { get; private set; }

    public Guid? DefaultEquipmentVariantId { get; private set; }
    public EquipmentVariant? DefaultEquipmentVariant { get; private set; }

    public IReadOnlyCollection<MuscleGroup> PrimaryMuscles {  get; private set; } = [];
    public IReadOnlyCollection<MuscleGroup> SecondaryMuscles {  get; private set; } = [];

    // For cardio exercises
    public CardioType? DefaultCardioType { get; private set; }

    private Exercise() { }

    public static Exercise Create(
        string name,
        ExerciseType type,
        string? description = null,
        Guid? defaultEquipmentId = null,
        Guid? defaultEquipmentVariantId = null,
        IEnumerable<MuscleGroup>? primaryMuscles = null,
        IEnumerable<MuscleGroup>? secondaryMuscles = null,
        CardioType? defaultCardioType = null,
        bool isSeeded = false) =>
        new()
        {
            Name = name.Trim(),
            Type = type,
            Description = description,
            DefaultEquipmentId = defaultEquipmentId,
            DefaultEquipmentVariantId = defaultEquipmentVariantId,
            DefaultCardioType = defaultCardioType,
            IsSeeded = isSeeded,
            PrimaryMuscles = primaryMuscles?.ToList() ?? [],
            SecondaryMuscles = secondaryMuscles?.ToList() ?? []
        };

    public void Update(
        string name,
        string? description,
        Guid? defaultEquipmentId,
        Guid? defaultEquipmentVariantId,
        IEnumerable<MuscleGroup>? primaryMuscles,
        IEnumerable<MuscleGroup>? secondaryMuscles)
    {
        Name = name.Trim();
        Description = description;
        DefaultEquipmentId = defaultEquipmentId;
        DefaultEquipmentVariantId = defaultEquipmentVariantId;
        PrimaryMuscles = primaryMuscles?.ToList() ?? [];
        SecondaryMuscles = secondaryMuscles?.ToList() ?? [];
        Touch();
    }
}