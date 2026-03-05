using Application.Common.Interfaces;
using Application.Features.TrainingPlans.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.Commands;

public sealed class AddCircuitBlockCommandHandler(
    IWorkoutDayRepository workoutDays,
    IExerciseRepository exercises,
    ICurrentUserService currentUser,
    IUnitOfWork uow) : IRequestHandler<AddCircuitBlockCommand, Result<WorkoutBlockDto>>
{
    public async Task<Result<WorkoutBlockDto>> Handle(AddCircuitBlockCommand request, CancellationToken ct)
    {
        var day = await workoutDays.GetByIdWithBlocksAsync(request.WorkoutDayId, ct);
        if (day is null) return Result.Fail(DomainErrors.WorkoutDay.NotFound(request.WorkoutDayId));
        if (day.WeekTemplate.TrainingPlan.OwnerId != currentUser.UserId)
            return Result.Fail(DomainErrors.General.Forbidden());

        var circuit = CircuitBlock.Create(
            request.WorkoutDayId, request.Rounds, request.RestBetweenRoundsSeconds, request.Notes);

        var circuitExDtos = new List<CircuitExerciseDto>();
        foreach (var exDto in request.Exercises)
        {
            var exercise = await exercises.GetByIdAsync(exDto.ExerciseId, ct);
            if (exercise is null) return Result.Fail(DomainErrors.Exercise.NotFound(exDto.ExerciseId));

            var ce = circuit.AddExercise(
                exDto.ExerciseId, exDto.MinReps, exDto.MaxReps, exDto.DurationSeconds,
                exDto.RestAfterSeconds, exDto.MovementModifier,
                exDto.EquipmentId, exDto.EquipmentVariantId, exDto.Notes);

            circuitExDtos.Add(new CircuitExerciseDto(
                ce.Id, ce.Order, ce.ExerciseId, exercise.Name,
                null, null, ce.MovementModifier,
                ce.MinReps, ce.MaxReps, ce.DurationSeconds, ce.RestAfterSeconds, ce.Notes));
        }

        day.AddBlock(circuit);
        await uow.SaveChangesAsync(ct);

        return Result.Ok(new WorkoutBlockDto(
            circuit.Id, BlockType.Circuit, circuit.Order, circuit.Notes,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null,
            circuit.Rounds, circuit.RestBetweenRoundsSeconds,
            circuitExDtos.AsReadOnly(),
            null, null, null, null, null, null, null));
    }
}