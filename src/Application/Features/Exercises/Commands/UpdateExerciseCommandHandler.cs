using Application.Common.Interfaces;
using Application.Features.Exercises.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Features.Exercises.Commands;

public sealed class UpdateExerciseCommandHandler(
    IExerciseRepository exercises,
    IUnitOfWork uow,
    HybridCache cache) : IRequestHandler<UpdateExerciseCommand, Result<ExerciseDto>>
{
    public async Task<Result<ExerciseDto>> Handle(UpdateExerciseCommand request, CancellationToken ct)
    {
        var exercise = await exercises.GetByIdAsync(request.Id, ct);
        if (exercise is null) return Result.Fail(DomainErrors.Exercise.NotFound(request.Id));
        if (exercise.IsSeeded) return Result.Fail(DomainErrors.General.Forbidden());

        exercise.Update(request.Name, request.Description,
            request.DefaultEquipmentId, request.DefaultEquipmentVariantId,
            request.PrimaryMuscles, request.SecondaryMuscles);

        await uow.SaveChangesAsync(ct);
        await cache.RemoveAsync($"exercise:{request.Id}", ct);
        await cache.RemoveByTagAsync("exercises", ct);

        return Result.Ok(new ExerciseDto(
            exercise.Id, exercise.Name, exercise.Type, exercise.Description, exercise.IsSeeded,
            exercise.DefaultEquipmentId, null, exercise.DefaultEquipmentVariantId, null,
            exercise.PrimaryMuscles, exercise.SecondaryMuscles, exercise.DefaultCardioType));
    }
}