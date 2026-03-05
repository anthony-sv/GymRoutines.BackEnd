using FluentResults;
using MediatR;

namespace Application.Features.Exercises.DTOs;

public sealed record GetEquipmentQuery : IRequest<Result<IReadOnlyList<EquipmentDto>>>;