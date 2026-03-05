using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record AddWeekTemplateCommand(Guid PlanId, int WeekNumber) : IRequest<Result<WeekTemplateDto>>;