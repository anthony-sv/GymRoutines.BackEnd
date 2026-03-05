using Domain.Enums;
using FluentResults;
using MediatR;

namespace Application.Features.TrainingPlans.DTOs;

public sealed record AddWorkoutDayCommand(
    Guid WeekTemplateId, int DayOfWeek, DayType DayType, string? Label, string? Notes)
    : IRequest<Result<WorkoutDaySummaryDto>>;