namespace Application.Features.TrainingPlans.DTOs;

public sealed record WeekTemplateDto(
    Guid Id,
    int WeekNumber,
    IReadOnlyList<WorkoutDaySummaryDto> Days);