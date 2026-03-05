using Application.Common;
using Application.Common.Interfaces;
using Application.Features.Exercises.DTOs;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Domain.Entities;

namespace Application.Features.Exercises.Queries;

public sealed class GetExercisesQueryHandler(
    IExerciseRepository exercises,
    HybridCache cache) : IRequestHandler<GetExercisesQuery, Result<PagedResponse<ExerciseDto>>>
{
    public async Task<Result<PagedResponse<ExerciseDto>>> Handle(
        GetExercisesQuery request, CancellationToken ct)
    {
        var cacheKey = $"exercises:page:{request.Page}:size:{request.PageSize}:search:{request.Search ?? "all"}";

        var result = await cache.GetOrCreateAsync(
            cacheKey,
            async token =>
            {
                var (items, total) = await exercises.GetPagedAsync(request.Search, request.Page, request.PageSize, token);
                var dtos = items.Select(MapToDto).ToList().AsReadOnly();
                return new PagedResponse<ExerciseDto>(dtos, request.Page, request.PageSize, total);
            },
            new HybridCacheEntryOptions { Expiration = TimeSpan.FromMinutes(10) },
            cancellationToken: ct);

        return Result.Ok(result!);
    }

    private static ExerciseDto MapToDto(Exercise e) => new(
        e.Id, e.Name, e.Type, e.Description, e.IsSeeded,
        e.DefaultEquipmentId, e.DefaultEquipment?.Name,
        e.DefaultEquipmentVariantId, e.DefaultEquipmentVariant?.Name,
        e.PrimaryMuscles, e.SecondaryMuscles, e.DefaultCardioType);
}