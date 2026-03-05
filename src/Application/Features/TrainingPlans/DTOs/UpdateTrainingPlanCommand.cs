using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record UpdateTrainingPlanCommand(
    Guid Id, string Name, string? Description, bool IsPublic)
    : IRequest<Result<TrainingPlanSummaryDto>>;