using Domain.Enums;
using FluentResults;
using MediatR;

namespace Application.Features.Exercises.DTOs;

public sealed record CreateExerciseCommand(
    string Name,
    ExerciseType Type,
    string? Description,
    Guid? DefaultEquipmentId,
    Guid? DefaultEquipmentVariantId,
    List<MuscleGroup> PrimaryMuscles,
    List<MuscleGroup> SecondaryMuscles,
    CardioType? DefaultCardioType) : IRequest<Result<ExerciseDto>>;