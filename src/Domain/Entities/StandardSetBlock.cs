using Domain.Enums;

namespace Domain.Entities;

// ─────────────────────────────────────────────────────────────────────────────
// STANDARD SET BLOCK
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// A straight set block: exercise × sets × rep/duration range × rest.
/// Supports intensity techniques: DropSet, RestPause, TUT.
/// </summary>
public sealed class StandardSetBlock : WorkoutBlock
{
    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = default!;

    // Equipment override (if user wants to override the exercise's default)
    public Guid? EquipmentId { get; private set; }
    public Equipment? Equipment { get; private set; }
    public Guid? EquipmentVariantId { get; private set; }
    public EquipmentVariant? EquipmentVariant { get; private set; }

    public MovementModifier MovementModifier { get; private set; }

    public int Sets { get; private set; }
    public int? MinReps { get; private set; }
    public int? MaxReps { get; private set; }

    /// <summary>For timed exercises (poses, planks): duration per set in seconds.</summary>
    public int? DurationSeconds { get; private set; }

    /// <summary>Rest between sets in seconds.</summary>
    public int RestSeconds { get; private set; }

    // Intensity technique
    public IntensityTechnique IntensityTechnique { get; private set; }
    public DropSetScope DropSetScope { get; private set; }

    /// <summary>
    /// Number of drops per drop-set (e.g. 2 = 3 total mini-sets per set).
    /// Only relevant when IntensityTechnique == DropSet.
    /// </summary>
    public int? DropCount { get; private set; }

    /// <summary>TUT tempo string, e.g. "3-1-3-0" (eccentric-pause-concentric-pause).</summary>
    public string? TutTempo { get; private set; }

    /// <summary>Rest-pause rest duration in seconds between mini-sets.</summary>
    public int? RestPauseSeconds { get; private set; }

    private StandardSetBlock() { }

    public static StandardSetBlock Create(
        Guid workoutDayId,
        Guid exerciseId,
        int sets,
        int restSeconds,
        int? minReps = null,
        int? maxReps = null,
        int? durationSeconds = null,
        MovementModifier movementModifier = MovementModifier.None,
        Guid? equipmentId = null,
        Guid? equipmentVariantId = null,
        IntensityTechnique intensityTechnique = IntensityTechnique.None,
        DropSetScope dropSetScope = DropSetScope.None,
        int? dropCount = null,
        string? tutTempo = null,
        int? restPauseSeconds = null,
        string? notes = null) =>
        new()
        {
            WorkoutDayId = workoutDayId,
            BlockType = BlockType.StandardSet,
            ExerciseId = exerciseId,
            Sets = sets,
            MinReps = minReps,
            MaxReps = maxReps,
            DurationSeconds = durationSeconds,
            RestSeconds = restSeconds,
            MovementModifier = movementModifier,
            EquipmentId = equipmentId,
            EquipmentVariantId = equipmentVariantId,
            IntensityTechnique = intensityTechnique,
            DropSetScope = dropSetScope,
            DropCount = dropCount,
            TutTempo = tutTempo,
            RestPauseSeconds = restPauseSeconds,
            Notes = notes
        };
}