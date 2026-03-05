using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Commands;

public sealed class AddWeekTemplateCommandHandler(
    ITrainingPlanRepository plans,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<AddWeekTemplateCommand, Result<WeekTemplateDto>>
{
    public async Task<Result<WeekTemplateDto>> Handle(AddWeekTemplateCommand request, CancellationToken ct)
    {
        var plan = await plans.GetByIdWithDetailsAsync(request.PlanId, ct);
        if (plan is null) return Result.Fail(DomainErrors.TrainingPlan.NotFound(request.PlanId));
        if (plan.OwnerId != currentUser.UserId) return Result.Fail(DomainErrors.General.Forbidden());

        var weekResult = plan.AddWeekTemplate(request.WeekNumber);
        if (weekResult.IsFailed) return weekResult.ToResult();

        await uow.SaveChangesAsync(ct);

        var week = weekResult.Value;
        return Result.Ok(new WeekTemplateDto(week.Id, week.WeekNumber, []));
    }
}