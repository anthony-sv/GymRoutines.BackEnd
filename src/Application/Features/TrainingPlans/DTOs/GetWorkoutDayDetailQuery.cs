using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record GetWorkoutDayDetailQuery(Guid Id) : IRequest<Result<WorkoutDayDetailDto>>;