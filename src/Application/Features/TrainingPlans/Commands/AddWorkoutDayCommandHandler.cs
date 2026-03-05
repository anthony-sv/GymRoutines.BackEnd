using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Commands;

public sealed class AddWorkoutDayCommandHandler(
    IWeekTemplateRepository weekTemplates,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<AddWorkoutDayCommand, Result<WorkoutDaySummaryDto>>
{
    public async Task<Result<WorkoutDaySummaryDto>> Handle(AddWorkoutDayCommand request, CancellationToken ct)
    {
        var week = await weekTemplates.GetByIdWithDetailsAsync(request.WeekTemplateId, ct);
        if (week is null) return Result.Fail(DomainErrors.WorkoutDay.NotFound(request.WeekTemplateId));
        if (week.TrainingPlan.OwnerId != currentUser.UserId) return Result.Fail(DomainErrors.General.Forbidden());

        var dayResult = week.AddDay(request.DayOfWeek, request.DayType, request.Notes);
        if (dayResult.IsFailed) return dayResult.ToResult();

        var day = dayResult.Value;
        if (request.Label is not null) day.SetLabel(request.Label);

        await uow.SaveChangesAsync(ct);
        return Result.Ok(new WorkoutDaySummaryDto(
            day.Id, day.DayOfWeek, day.DayType, day.Label, day.Notes, 0));
    }
}