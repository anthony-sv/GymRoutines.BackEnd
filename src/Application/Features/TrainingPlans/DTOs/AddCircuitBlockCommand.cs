using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record AddCircuitBlockCommand(
    Guid WorkoutDayId,
    int Rounds,
    int RestBetweenRoundsSeconds,
    string? Notes,
    List<AddCircuitExerciseDto> Exercises) : IRequest<Result<WorkoutBlockDto>>;