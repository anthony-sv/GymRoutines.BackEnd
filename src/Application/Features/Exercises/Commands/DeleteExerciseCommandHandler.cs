using Application.Common.Interfaces;
using Application.Features.Exercises.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Features.Exercises.Commands;

public sealed class DeleteExerciseCommandHandler(
    IExerciseRepository exercises,
    IUnitOfWork uow,
    HybridCache cache) : IRequestHandler<DeleteExerciseCommand, Result>
{
    public async Task<Result> Handle(DeleteExerciseCommand request, CancellationToken ct)
    {
        var exercise = await exercises.GetByIdAsync(request.Id, ct);
        if (exercise is null) return Result.Fail(DomainErrors.Exercise.NotFound(request.Id));
        if (exercise.IsSeeded) return Result.Fail(DomainErrors.General.Forbidden());

        exercises.Remove(exercise);
        await uow.SaveChangesAsync(ct);
        await cache.RemoveAsync($"exercise:{request.Id}", ct);
        await cache.RemoveByTagAsync("exercises", ct);

        return Result.Ok();
    }
}