using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// One exercise entry inside a circuit block.
/// </summary>
public sealed class CircuitExercise : BaseEntity
{
    public Guid CircuitBlockId { get; private set; }
    public CircuitBlock CircuitBlock { get; private set; } = default!;

    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = default!;

    public int Order { get; private set; }

    public int? MinReps { get; private set; }
    public int? MaxReps { get; private set; }

    /// <summary>Duration for timed/flex exercises (in seconds).</summary>
    public int? DurationSeconds { get; private set; }

    /// <summary>Optional brief rest after this specific exercise within the round.</summary>
    public int? RestAfterSeconds { get; private set; }

    public MovementModifier MovementModifier { get; private set; }

    public Guid? EquipmentId { get; private set; }
    public Equipment? Equipment { get; private set; }
    public Guid? EquipmentVariantId { get; private set; }
    public EquipmentVariant? EquipmentVariant { get; private set; }

    public string? Notes { get; private set; }

    private CircuitExercise() { }

    internal static CircuitExercise Create(
        Guid circuitBlockId,
        Guid exerciseId,
        int order,
        int? minReps,
        int? maxReps,
        int? durationSeconds,
        int? restAfterSeconds,
        MovementModifier movementModifier,
        Guid? equipmentId,
        Guid? equipmentVariantId,
        string? notes) =>
        new()
        {
            CircuitBlockId = circuitBlockId,
            ExerciseId = exerciseId,
            Order = order,
            MinReps = minReps,
            MaxReps = maxReps,
            DurationSeconds = durationSeconds,
            RestAfterSeconds = restAfterSeconds,
            MovementModifier = movementModifier,
            EquipmentId = equipmentId,
            EquipmentVariantId = equipmentVariantId,
            Notes = notes
        };
}