using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record GetTrainingPlanDetailQuery(Guid Id) : IRequest<Result<TrainingPlanDetailDto>>;