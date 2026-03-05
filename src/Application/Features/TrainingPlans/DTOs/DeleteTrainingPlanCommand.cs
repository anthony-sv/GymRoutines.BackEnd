using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record DeleteTrainingPlanCommand(Guid Id) : IRequest<Result>;