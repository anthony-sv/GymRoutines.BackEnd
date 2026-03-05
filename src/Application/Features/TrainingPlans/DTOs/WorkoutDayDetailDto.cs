using Domain.Enums;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record WorkoutDayDetailDto(
    Guid Id,
    int DayOfWeek,
    DayType DayType,
    string? Label,
    string? Notes,
    IReadOnlyList<WorkoutBlockDto> Blocks);