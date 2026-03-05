using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Domain.Entities;

namespace Application.Features.TrainingPlans.Queries;

public sealed class GetTrainingPlanDetailQueryHandler(
    ITrainingPlanRepository plans,
    ICurrentUserService currentUser) : IRequestHandler<GetTrainingPlanDetailQuery, Result<TrainingPlanDetailDto>>
{
    public async Task<Result<TrainingPlanDetailDto>> Handle(
        GetTrainingPlanDetailQuery request, CancellationToken ct)
    {
        var plan = await plans.GetByIdWithDetailsAsync(request.Id, ct);
        if (plan is null) return Result.Fail(DomainErrors.TrainingPlan.NotFound(request.Id));

        if (!plan.IsPublic && plan.OwnerId != currentUser.UserId)
            return Result.Fail(DomainErrors.General.Forbidden());

        return Result.Ok(MapToDetail(plan));
    }

    private static TrainingPlanDetailDto MapToDetail(TrainingPlan p) => new(
        p.Id, p.Name, p.Description, p.TotalWeeks, p.StructureType,
        p.OwnerId, p.IsPublic,
        p.WeekTemplates.OrderBy(w => w.WeekNumber)
            .Select(w => new WeekTemplateDto(
                w.Id, w.WeekNumber,
                w.Days.OrderBy(d => d.DayOfWeek)
                    .Select(d => new WorkoutDaySummaryDto(
                        d.Id, d.DayOfWeek, d.DayType, d.Label, d.Notes, d.Blocks.Count))
                    .ToList().AsReadOnly()))
            .ToList().AsReadOnly(),
        p.CreatedAt, p.UpdatedAt);
}