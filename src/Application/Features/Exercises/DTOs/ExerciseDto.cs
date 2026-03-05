using Domain.Enums;

namespace Application.Features.Exercises.DTOs;

public sealed record ExerciseDto(
    Guid Id,
    string Name,
    ExerciseType Type,
    string? Description,
    bool IsSeeded,
    Guid? DefaultEquipmentId,
    string? DefaultEquipmentName,
    Guid? DefaultEquipmentVariantId,
    string? DefaultEquipmentVariantName,
    IReadOnlyCollection<MuscleGroup> PrimaryMuscles,
    IReadOnlyCollection<MuscleGroup> SecondaryMuscles,
    CardioType? DefaultCardioType);