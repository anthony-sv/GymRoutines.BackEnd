using Domain.Common;
using Domain.Enums;
using Domain.Errors;

using FluentResults;

namespace Domain.Entities;

/// <summary>
/// The top-level training program (e.g. "8-Week Arm Focus").
/// Owns the week schedule(s).
/// </summary>
public sealed class TrainingPlan : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public int TotalWeeks { get; private set; }
    public ProgramStructureType StructureType { get; private set; }
    public Guid OwnerId { get; private set; }
    public User Owner { get; private set; } = default!;
    public bool IsPublic { get; private set; }

    private readonly List<WeekTemplate> _weekTemplates = [];

    /// <summary>
    /// If StructureType == RepeatingWeek: exactly 1 entry.
    /// If StructureType == FullSchedule: exactly TotalWeeks entries.
    /// </summary>
    public IReadOnlyCollection<WeekTemplate> WeekTemplates => _weekTemplates.AsReadOnly();

    private TrainingPlan() { }

    public static Result<TrainingPlan> Create(
        string name,
        string? description,
        int totalWeeks,
        ProgramStructureType structureType,
        Guid ownerId,
        bool isPublic = false)
    {
        if (totalWeeks < 1)
            return Result.Fail(DomainErrors.General.Validation("Total weeks must be at least 1."));

        return new TrainingPlan
        {
            Name = name.Trim(),
            Description = description,
            TotalWeeks = totalWeeks,
            StructureType = structureType,
            OwnerId = ownerId,
            IsPublic = isPublic
        };
    }

    public Result<WeekTemplate> AddWeekTemplate(int weekNumber)
    {
        if (StructureType == ProgramStructureType.RepeatingWeek && _weekTemplates.Count >= 1)
            return Result.Fail(DomainErrors.General.Validation(
                "A repeating-week plan can only have one week template."));

        if (StructureType == ProgramStructureType.FullSchedule && _weekTemplates.Count >= TotalWeeks)
            return Result.Fail(DomainErrors.General.Validation(
                $"This plan already has {TotalWeeks} week templates defined."));

        if (_weekTemplates.Any(w => w.WeekNumber == weekNumber))
            return Result.Fail(DomainErrors.General.Conflict(
                $"Week {weekNumber} already exists."));

        var week = WeekTemplate.Create(Id, weekNumber);
        _weekTemplates.Add(week);
        Touch();
        return Result.Ok(week);
    }

    public void Update(string name, string? description, bool isPublic)
    {
        Name = name.Trim();
        Description = description;
        IsPublic = isPublic;
        Touch();
    }
}