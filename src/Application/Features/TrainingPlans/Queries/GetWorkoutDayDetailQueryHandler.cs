using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Queries;

public sealed class GetWorkoutDayDetailQueryHandler(
    IWorkoutDayRepository workoutDays,
    ICurrentUserService currentUser) : IRequestHandler<GetWorkoutDayDetailQuery, Result<WorkoutDayDetailDto>>
{
    public async Task<Result<WorkoutDayDetailDto>> Handle(GetWorkoutDayDetailQuery request, CancellationToken ct)
    {
        var day = await workoutDays.GetByIdWithBlocksAsync(request.Id, ct);
        if (day is null) return Result.Fail(DomainErrors.WorkoutDay.NotFound(request.Id));

        var plan = day.WeekTemplate.TrainingPlan;
        if (!plan.IsPublic && plan.OwnerId != currentUser.UserId)
            return Result.Fail(DomainErrors.General.Forbidden());

        var blockDtos = day.Blocks.OrderBy(b => b.Order).Select(MapBlock).ToList().AsReadOnly();
        return Result.Ok(new WorkoutDayDetailDto(
            day.Id, day.DayOfWeek, day.DayType, day.Label, day.Notes, blockDtos));
    }

    private static WorkoutBlockDto MapBlock(WorkoutBlock b) => b switch
    {
        StandardSetBlock s => new WorkoutBlockDto(
            s.Id, BlockType.StandardSet, s.Order, s.Notes,
            s.ExerciseId, s.Exercise?.Name, s.Equipment?.Name, s.EquipmentVariant?.Name,
            s.MovementModifier, s.Sets, s.MinReps, s.MaxReps, s.DurationSeconds, s.RestSeconds,
            s.IntensityTechnique, s.DropSetScope, s.DropCount, s.TutTempo, s.RestPauseSeconds,
            null, null, null, null, null, null, null, null, null, null),

        CircuitBlock c => new WorkoutBlockDto(
            c.Id, BlockType.Circuit, c.Order, c.Notes,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null,
            c.Rounds, c.RestBetweenRoundsSeconds,
            c.Exercises.OrderBy(e => e.Order).Select(ce => new CircuitExerciseDto(
                ce.Id, ce.Order, ce.ExerciseId, ce.Exercise?.Name ?? "",
                ce.Equipment?.Name, ce.EquipmentVariant?.Name,
                ce.MovementModifier, ce.MinReps, ce.MaxReps,
                ce.DurationSeconds, ce.RestAfterSeconds, ce.Notes))
            .ToList().AsReadOnly(),
            null, null, null, null, null, null, null),

        CardioBlock card => new WorkoutBlockDto(
            card.Id, BlockType.Cardio, card.Order, card.Notes,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            card.CardioType, card.DurationMinutes,
            card.Speed, card.Incline, card.Resistance,
            card.TargetCalories, card.TargetHeartRateBpm),

        _ => throw new InvalidOperationException($"Unknown block type: {b.GetType().Name}")
    };
}