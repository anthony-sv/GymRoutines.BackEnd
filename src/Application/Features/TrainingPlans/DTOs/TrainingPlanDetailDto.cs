using Domain.Enums;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record TrainingPlanDetailDto(
    Guid Id,
    string Name,
    string? Description,
    int TotalWeeks,
    ProgramStructureType StructureType,
    Guid OwnerId,
    bool IsPublic,
    IReadOnlyList<WeekTemplateDto> WeekTemplates,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);