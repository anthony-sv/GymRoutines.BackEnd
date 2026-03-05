using Domain.Enums;
using FluentResults;
using MediatR;

namespace Application.Features.Exercises.DTOs;

public sealed record UpdateExerciseCommand(
    Guid Id,
    string Name,
    string? Description,
    Guid? DefaultEquipmentId,
    Guid? DefaultEquipmentVariantId,
    List<MuscleGroup> PrimaryMuscles,
    List<MuscleGroup> SecondaryMuscles) : IRequest<Result<ExerciseDto>>;