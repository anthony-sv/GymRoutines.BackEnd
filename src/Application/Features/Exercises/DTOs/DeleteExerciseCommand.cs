using FluentResults;
using MediatR;

namespace Application.Features.Exercises.DTOs;

public sealed record DeleteExerciseCommand(Guid Id) : IRequest<Result>;