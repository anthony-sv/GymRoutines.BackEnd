using Domain.Enums;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record AddCardioBlockCommand(
    Guid WorkoutDayId,
    CardioType CardioType,
    int DurationMinutes,
    decimal? Speed,
    decimal? Incline,
    decimal? Resistance,
    int? TargetCalories,
    int? TargetHeartRateBpm,
    Guid? EquipmentId,
    string? Notes) : IRequest<Result<WorkoutBlockDto>>;