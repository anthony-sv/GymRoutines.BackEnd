using Application.Common;
using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Domain.Entities;

namespace Application.Features.TrainingPlans.Queries;

public sealed class GetTrainingPlansQueryHandler(
    ITrainingPlanRepository plans,
    ICurrentUserService currentUser) : IRequestHandler<GetTrainingPlansQuery, Result<PagedResponse<TrainingPlanSummaryDto>>>
{
    public async Task<Result<PagedResponse<TrainingPlanSummaryDto>>> Handle(
        GetTrainingPlansQuery request, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result.Fail(DomainErrors.General.Unauthorized());

        var (items, total) = await plans.GetPagedAsync(
            currentUser.UserId, request.IncludePublic, request.Page, request.PageSize, ct);

        var dtos = items.Select(MapToSummary).ToList().AsReadOnly();
        return Result.Ok(new PagedResponse<TrainingPlanSummaryDto>(dtos, request.Page, request.PageSize, total));
    }

    private static TrainingPlanSummaryDto MapToSummary(TrainingPlan p) => new(
        p.Id, p.Name, p.Description, p.TotalWeeks, p.StructureType,
        p.OwnerId, p.IsPublic, p.CreatedAt, p.UpdatedAt);
}