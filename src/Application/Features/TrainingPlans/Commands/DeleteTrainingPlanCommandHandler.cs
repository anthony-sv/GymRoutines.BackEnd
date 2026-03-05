using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Commands;

public sealed class DeleteTrainingPlanCommandHandler(
    ITrainingPlanRepository plans,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<DeleteTrainingPlanCommand, Result>
{
    public async Task<Result> Handle(DeleteTrainingPlanCommand request, CancellationToken ct)
    {
        var plan = await plans.GetByIdAsync(request.Id, ct);
        if (plan is null) return Result.Fail(DomainErrors.TrainingPlan.NotFound(request.Id));
        if (plan.OwnerId != currentUser.UserId) return Result.Fail(DomainErrors.General.Forbidden());

        plans.Remove(plan);
        await uow.SaveChangesAsync(ct);
        return Result.Ok();
    }
}