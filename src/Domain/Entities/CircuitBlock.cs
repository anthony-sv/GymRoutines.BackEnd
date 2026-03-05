using Domain.Enums;

namespace Domain.Entities;

// ─────────────────────────────────────────────────────────────────────────────
// CIRCUIT BLOCK
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// A circuit/series: a sequence of exercises repeated N rounds,
/// with a rest between each round.
/// </summary>
public sealed class CircuitBlock : WorkoutBlock
{
    public int Rounds { get; private set; }

    /// <summary>Rest in seconds between rounds.</summary>
    public int RestBetweenRoundsSeconds { get; private set; }

    private readonly List<CircuitExercise> _exercises = [];
    public IReadOnlyCollection<CircuitExercise> Exercises => _exercises.AsReadOnly();

    private CircuitBlock() { }

    public static CircuitBlock Create(
        Guid workoutDayId,
        int rounds,
        int restBetweenRoundsSeconds,
        string? notes = null) =>
        new()
        {
            WorkoutDayId = workoutDayId,
            BlockType = BlockType.Circuit,
            Rounds = rounds,
            RestBetweenRoundsSeconds = restBetweenRoundsSeconds,
            Notes = notes
        };

    public CircuitExercise AddExercise(
        Guid exerciseId,
        int? minReps = null,
        int? maxReps = null,
        int? durationSeconds = null,
        int? restAfterSeconds = null,
        MovementModifier movementModifier = MovementModifier.None,
        Guid? equipmentId = null,
        Guid? equipmentVariantId = null,
        string? notes = null)
    {
        var order = _exercises.Count > 0 ? _exercises.Max(e => e.Order) + 1 : 1;
        var exercise = CircuitExercise.Create(
            Id, exerciseId, order, minReps, maxReps, durationSeconds,
            restAfterSeconds, movementModifier, equipmentId, equipmentVariantId, notes);
        _exercises.Add(exercise);
        Touch();
        return exercise;
    }
}