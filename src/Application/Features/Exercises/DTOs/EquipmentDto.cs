using Domain.Enums;

namespace Application.Features.Exercises.DTOs;

public sealed record EquipmentDto(
    Guid Id,
    string Name,
    EquipmentCategory Category,
    string? Description,
    bool IsSeeded,
    IReadOnlyCollection<EquipmentVariantDto> Variants);