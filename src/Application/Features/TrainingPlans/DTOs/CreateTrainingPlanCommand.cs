using Domain.Enums;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record CreateTrainingPlanCommand(
    string Name,
    string? Description,
    int TotalWeeks,
    ProgramStructureType StructureType,
    bool IsPublic = false) : IRequest<Result<TrainingPlanSummaryDto>>;