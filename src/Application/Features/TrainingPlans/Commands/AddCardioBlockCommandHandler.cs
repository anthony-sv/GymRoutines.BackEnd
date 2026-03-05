using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Commands;

public sealed class AddCardioBlockCommandHandler(
    IWorkoutDayRepository workoutDays,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<AddCardioBlockCommand, Result<WorkoutBlockDto>>
{
    public async Task<Result<WorkoutBlockDto>> Handle(AddCardioBlockCommand request, CancellationToken ct)
    {
        var day = await workoutDays.GetByIdWithBlocksAsync(request.WorkoutDayId, ct);
        if (day is null) return Result.Fail(DomainErrors.WorkoutDay.NotFound(request.WorkoutDayId));
        if (day.WeekTemplate.TrainingPlan.OwnerId != currentUser.UserId)
            return Result.Fail(DomainErrors.General.Forbidden());

        var block = CardioBlock.Create(
            request.WorkoutDayId, request.CardioType, request.DurationMinutes,
            request.Speed, request.Incline, request.Resistance,
            request.TargetCalories, request.TargetHeartRateBpm, request.EquipmentId, request.Notes);

        day.AddBlock(block);
        await uow.SaveChangesAsync(ct);

        return Result.Ok(new WorkoutBlockDto(
            block.Id, BlockType.Cardio, block.Order, block.Notes,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            block.CardioType, block.DurationMinutes,
            block.Speed, block.Incline, block.Resistance,
            block.TargetCalories, block.TargetHeartRateBpm));
    }
}