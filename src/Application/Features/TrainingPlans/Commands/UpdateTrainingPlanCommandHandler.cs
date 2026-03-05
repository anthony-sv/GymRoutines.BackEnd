using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Commands;

public sealed class UpdateTrainingPlanCommandHandler(
    ITrainingPlanRepository plans,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<UpdateTrainingPlanCommand, Result<TrainingPlanSummaryDto>>
{
    public async Task<Result<TrainingPlanSummaryDto>> Handle(
        UpdateTrainingPlanCommand request, CancellationToken ct)
    {
        var plan = await plans.GetByIdAsync(request.Id, ct);
        if (plan is null) return Result.Fail(DomainErrors.TrainingPlan.NotFound(request.Id));
        if (plan.OwnerId != currentUser.UserId) return Result.Fail(DomainErrors.General.Forbidden());

        plan.Update(request.Name, request.Description, request.IsPublic);
        await uow.SaveChangesAsync(ct);

        return Result.Ok(new TrainingPlanSummaryDto(
            plan.Id, plan.Name, plan.Description, plan.TotalWeeks,
            plan.StructureType, plan.OwnerId, plan.IsPublic,
            plan.CreatedAt, plan.UpdatedAt));
    }
}