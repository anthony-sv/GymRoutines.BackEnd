using Application.Common.Interfaces;
using Application.Features.Exercises.DTOs;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Features.Exercises.Queries;

public sealed class GetEquipmentQueryHandler(
    IEquipmentRepository equipmentRepo,
    HybridCache cache) : IRequestHandler<GetEquipmentQuery, Result<IReadOnlyList<EquipmentDto>>>
{
    public async Task<Result<IReadOnlyList<EquipmentDto>>> Handle(GetEquipmentQuery request, CancellationToken ct)
    {
        var result = await cache.GetOrCreateAsync(
            "equipment:all",
            async token =>
            {
                var items = await equipmentRepo.GetAllAsync(token);
                return items.Select(e => new EquipmentDto(
                    e.Id, e.Name, e.Category, e.Description, e.IsSeeded,
                    e.Variants.Select(v => new EquipmentVariantDto(
                        v.Id, v.EquipmentId, v.Name, v.Description, v.IsSeeded))
                    .ToList().AsReadOnly())).ToList().AsReadOnly() as IReadOnlyList<EquipmentDto>;
            },
            new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) },
            cancellationToken: ct);

        return Result.Ok(result!);
    }
}