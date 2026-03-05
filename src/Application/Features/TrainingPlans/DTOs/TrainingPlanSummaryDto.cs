using Domain.Enums;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record TrainingPlanSummaryDto(
    Guid Id,
    string Name,
    string? Description,
    int TotalWeeks,
    ProgramStructureType StructureType,
    Guid OwnerId,
    bool IsPublic,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);