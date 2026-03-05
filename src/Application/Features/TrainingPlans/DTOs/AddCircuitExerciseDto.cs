using Domain.Enums;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record AddCircuitExerciseDto(
    Guid ExerciseId,
    int? MinReps,
    int? MaxReps,
    int? DurationSeconds,
    int? RestAfterSeconds,
    MovementModifier MovementModifier,
    Guid? EquipmentId,
    Guid? EquipmentVariantId,
    string? Notes);