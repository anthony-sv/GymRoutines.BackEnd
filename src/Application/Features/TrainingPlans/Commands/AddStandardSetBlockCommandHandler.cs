using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Commands;

public sealed class AddStandardSetBlockCommandHandler(
    IWorkoutDayRepository workoutDays,
    IExerciseRepository exercises,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<AddStandardSetBlockCommand, Result<WorkoutBlockDto>>
{
    public async Task<Result<WorkoutBlockDto>> Handle(AddStandardSetBlockCommand request, CancellationToken ct)
    {
        var day = await workoutDays.GetByIdWithBlocksAsync(request.WorkoutDayId, ct);
        if (day is null) return Result.Fail(DomainErrors.WorkoutDay.NotFound(request.WorkoutDayId));
        if (day.WeekTemplate.TrainingPlan.OwnerId != currentUser.UserId)
            return Result.Fail(DomainErrors.General.Forbidden());

        var exercise = await exercises.GetByIdAsync(request.ExerciseId, ct);
        if (exercise is null) return Result.Fail(DomainErrors.Exercise.NotFound(request.ExerciseId));

        var block = StandardSetBlock.Create(
            request.WorkoutDayId, request.ExerciseId, request.Sets, request.RestSeconds,
            request.MinReps, request.MaxReps, request.DurationSeconds,
            request.MovementModifier, request.EquipmentId, request.EquipmentVariantId,
            request.IntensityTechnique, request.DropSetScope, request.DropCount,
            request.TutTempo, request.RestPauseSeconds, request.Notes);

        day.AddBlock(block);
        await uow.SaveChangesAsync(ct);

        return Result.Ok(new WorkoutBlockDto(
            block.Id, BlockType.StandardSet, block.Order, block.Notes,
            block.ExerciseId, exercise.Name, null, null,
            block.MovementModifier, block.Sets, block.MinReps, block.MaxReps,
            block.DurationSeconds, block.RestSeconds,
            block.IntensityTechnique, block.DropSetScope, block.DropCount,
            block.TutTempo, block.RestPauseSeconds,
            null, null, null, null, null, null, null, null, null, null));
    }
}