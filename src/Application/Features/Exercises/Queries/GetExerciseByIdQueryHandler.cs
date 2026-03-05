using Application.Common.Interfaces;
using Application.Features.Exercises.DTOs;
using Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Domain.Entities;

namespace Application.Features.Exercises.Queries;

public sealed class GetExerciseByIdQueryHandler(
    IExerciseRepository exercises,
    HybridCache cache) : IRequestHandler<GetExerciseByIdQuery, Result<ExerciseDto>>
{
    public async Task<Result<ExerciseDto>> Handle(GetExerciseByIdQuery request, CancellationToken ct)
    {
        var result = await cache.GetOrCreateAsync(
            $"exercise:{request.Id}",
            async token =>
            {
                var exercise = await exercises.GetByIdAsync(request.Id, token);
                if (exercise is null) return null;
                return MapToDto(exercise);
            },
            new HybridCacheEntryOptions { Expiration = TimeSpan.FromMinutes(10) },
            cancellationToken: ct);

        if (result is null)
            return Result.Fail(DomainErrors.Exercise.NotFound(request.Id));

        return Result.Ok(result);
    }

    private static ExerciseDto MapToDto(Exercise e) => new(
        e.Id, e.Name, e.Type, e.Description, e.IsSeeded,
        e.DefaultEquipmentId, e.DefaultEquipment?.Name,
        e.DefaultEquipmentVariantId, e.DefaultEquipmentVariant?.Name,
        e.PrimaryMuscles, e.SecondaryMuscles, e.DefaultCardioType);
}