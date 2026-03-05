using FluentResults;
using MediatR;

namespace Application.Features.Exercises.DTOs;

public sealed record GetExerciseByIdQuery(Guid Id) : IRequest<Result<ExerciseDto>>;