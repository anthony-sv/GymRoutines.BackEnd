using Domain.Enums;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record CircuitExerciseDto(
    Guid Id,
    int Order,
    Guid ExerciseId,
    string ExerciseName,
    string? EquipmentName,
    string? EquipmentVariantName,
    MovementModifier MovementModifier,
    int? MinReps,
    int? MaxReps,
    int? DurationSeconds,
    int? RestAfterSeconds,
    string? Notes);