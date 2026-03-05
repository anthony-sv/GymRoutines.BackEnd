using Domain.Common;
using Domain.Enums;
using Domain.Errors;

using FluentResults;

namespace Domain.Entities;

/// <summary>
/// One week's schedule — contains 7 workout days (Mon–Sun = 0–6).
/// </summary>
public sealed class WeekTemplate : BaseEntity
{
    public Guid TrainingPlanId { get; private set; }
    public TrainingPlan TrainingPlan { get; private set; } = default!;

    /// <summary>1-based week number within the plan.</summary>
    public int WeekNumber { get; private set; }

    private readonly List<WorkoutDay> _days = [];
    public IReadOnlyCollection<WorkoutDay> Days => _days.AsReadOnly();

    private WeekTemplate() { }

    internal static WeekTemplate Create(Guid trainingPlanId, int weekNumber) =>
        new() { TrainingPlanId = trainingPlanId, WeekNumber = weekNumber };

    public Result<WorkoutDay> AddDay(int dayOfWeek, DayType dayType, string? notes = null)
    {
        if (dayOfWeek < 0 || dayOfWeek > 6)
            return Result.Fail(DomainErrors.General.Validation("Day of week must be 0 (Monday) to 6 (Sunday)."));

        if (_days.Any(d => d.DayOfWeek == dayOfWeek))
            return Result.Fail(DomainErrors.TrainingPlan.DayOfWeekDuplicate(dayOfWeek));

        var day = WorkoutDay.Create(Id, dayOfWeek, dayType, notes);
        _days.Add(day);
        Touch();
        return Result.Ok(day);
    }
}