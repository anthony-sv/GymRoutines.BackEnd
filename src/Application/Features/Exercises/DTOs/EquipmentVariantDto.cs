namespace Application.Features.Exercises.DTOs;

public sealed record EquipmentVariantDto(
    Guid Id,
    Guid EquipmentId,
    string Name,
    string? Description,
    bool IsSeeded);