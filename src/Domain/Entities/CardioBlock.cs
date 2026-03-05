using Domain.Enums;

namespace Domain.Entities;

// ─────────────────────────────────────────────────────────────────────────────
// CARDIO BLOCK
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// A cardio session block — used for active rest days and optional cardio finishers.
/// </summary>
public sealed class CardioBlock : WorkoutBlock
{
    public CardioType CardioType { get; private set; }
    public int DurationMinutes { get; private set; }

    // Machine settings (all optional)
    public decimal? Speed { get; private set; }
    public decimal? Incline { get; private set; }
    public decimal? Resistance { get; private set; }

    // Targets (optional)
    public int? TargetCalories { get; private set; }
    public int? TargetHeartRateBpm { get; private set; }

    // Reference to optional equipment (e.g. Stairmaster model)
    public Guid? EquipmentId { get; private set; }
    public Equipment? Equipment { get; private set; }

    private CardioBlock() { }

    public static CardioBlock Create(
        Guid workoutDayId,
        CardioType cardioType,
        int durationMinutes,
        decimal? speed = null,
        decimal? incline = null,
        decimal? resistance = null,
        int? targetCalories = null,
        int? targetHeartRateBpm = null,
        Guid? equipmentId = null,
        string? notes = null) =>
        new()
        {
            WorkoutDayId = workoutDayId,
            BlockType = BlockType.Cardio,
            CardioType = cardioType,
            DurationMinutes = durationMinutes,
            Speed = speed,
            Incline = incline,
            Resistance = resistance,
            TargetCalories = targetCalories,
            TargetHeartRateBpm = targetHeartRateBpm,
            EquipmentId = equipmentId,
            Notes = notes
        };
}