using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Domain.Entities;

namespace Application.Features.TrainingPlans.Commands;

public sealed class CreateTrainingPlanCommandHandler(
    ITrainingPlanRepository plans,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<CreateTrainingPlanCommand, Result<TrainingPlanSummaryDto>>
{
    public async Task<Result<TrainingPlanSummaryDto>> Handle(
        CreateTrainingPlanCommand request, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result.Fail(DomainErrors.General.Unauthorized());

        var planResult = TrainingPlan.Create(
            request.Name, request.Description, request.TotalWeeks,
            request.StructureType, currentUser.UserId.Value, request.IsPublic);

        if (planResult.IsFailed) return planResult.ToResult();

        var plan = planResult.Value;
        await plans.AddAsync(plan, ct);
        await uow.SaveChangesAsync(ct);

        return Result.Ok(new TrainingPlanSummaryDto(
            plan.Id, plan.Name, plan.Description, plan.TotalWeeks,
            plan.StructureType, plan.OwnerId, plan.IsPublic,
            plan.CreatedAt, plan.UpdatedAt));
    }
}