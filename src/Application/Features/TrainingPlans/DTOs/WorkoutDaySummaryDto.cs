using Domain.Enums;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record WorkoutDaySummaryDto(
    Guid Id,
    int DayOfWeek,
    DayType DayType,
    string? Label,
    string? Notes,
    int BlockCount);