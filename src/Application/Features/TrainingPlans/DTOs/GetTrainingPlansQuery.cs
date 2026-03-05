using Application.Common;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record GetTrainingPlansQuery(int Page = 1, int PageSize = 20, bool IncludePublic = false)
    : IRequest<Result<PagedResponse<TrainingPlanSummaryDto>>>;