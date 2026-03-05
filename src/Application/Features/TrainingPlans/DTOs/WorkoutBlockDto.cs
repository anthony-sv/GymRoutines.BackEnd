using Domain.Enums;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record WorkoutBlockDto(
    Guid Id,
    BlockType BlockType,
    int Order,
    string? Notes,
    // StandardSet fields
    Guid? ExerciseId,
    string? ExerciseName,
    string? EquipmentName,
    string? EquipmentVariantName,
    MovementModifier? MovementModifier,
    int? Sets,
    int? MinReps,
    int? MaxReps,
    int? DurationSeconds,
    int? RestSeconds,
    IntensityTechnique? IntensityTechnique,
    DropSetScope? DropSetScope,
    int? DropCount,
    string? TutTempo,
    int? RestPauseSeconds,
    // Circuit fields
    int? Rounds,
    int? RestBetweenRoundsSeconds,
    IReadOnlyList<CircuitExerciseDto>? CircuitExercises,
    // Cardio fields
    CardioType? CardioType,
    int? DurationMinutes,
    decimal? Speed,
    decimal? Incline,
    decimal? Resistance,
    int? TargetCalories,
    int? TargetHeartRateBpm);