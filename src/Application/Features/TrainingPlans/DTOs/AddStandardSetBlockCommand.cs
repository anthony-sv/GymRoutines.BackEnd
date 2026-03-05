using Domain.Enums;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record AddStandardSetBlockCommand(
    Guid WorkoutDayId,
    Guid ExerciseId,
    int Sets,
    int RestSeconds,
    int? MinReps,
    int? MaxReps,
    int? DurationSeconds,
    MovementModifier MovementModifier,
    Guid? EquipmentId,
    Guid? EquipmentVariantId,
    IntensityTechnique IntensityTechnique,
    DropSetScope DropSetScope,
    int? DropCount,
    string? TutTempo,
    int? RestPauseSeconds,
    string? Notes) : IRequest<Result<WorkoutBlockDto>>;