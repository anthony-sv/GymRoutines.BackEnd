using Application.Common;
using FluentResults;
using MediatR;

namespace Application.Features.Exercises.DTOs;

public sealed record GetExercisesQuery(string? Search, int Page = 1, int PageSize = 50)
    : IRequest<Result<PagedResponse<ExerciseDto>>>;