using Application.Common.Interfaces;
using Application.Features.Exercises.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Domain.Entities;

namespace Application.Features.Exercises.Commands;

public sealed class CreateExerciseCommandHandler(
    IExerciseRepository exercises,
    IEquipmentRepository equipmentRepo,
    IUnitOfWork uow,
    HybridCache cache) : IRequestHandler<CreateExerciseCommand, Result<ExerciseDto>>
{
    public async Task<Result<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken ct)
    {
        if (await exercises.ExistsByNameAsync(request.Name, ct))
            return Result.Fail(DomainErrors.Exercise.NameAlreadyExists(request.Name));

        if (request.DefaultEquipmentId.HasValue)
        {
            var eq = await equipmentRepo.GetByIdAsync(request.DefaultEquipmentId.Value, ct);
            if (eq is null) return Result.Fail(DomainErrors.Equipment.NotFound(request.DefaultEquipmentId.Value));
        }

        var exercise = Exercise.Create(
            request.Name, request.Type, request.Description,
            request.DefaultEquipmentId, request.DefaultEquipmentVariantId,
            request.PrimaryMuscles, request.SecondaryMuscles,
            request.DefaultCardioType);

        await exercises.AddAsync(exercise, ct);
        await uow.SaveChangesAsync(ct);

        // Invalidate list caches
        await cache.RemoveByTagAsync("exercises", ct);

        return Result.Ok(new ExerciseDto(
            exercise.Id, exercise.Name, exercise.Type, exercise.Description, exercise.IsSeeded,
            exercise.DefaultEquipmentId, null,
            exercise.DefaultEquipmentVariantId, null,
            exercise.PrimaryMuscles, exercise.SecondaryMuscles, exercise.DefaultCardioType));
    }
}