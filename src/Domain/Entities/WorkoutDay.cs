using Domain.Common;
using Domain.Enums;
using Domain.Errors;

using FluentResults;

namespace Domain.Entities;

/// <summary>
/// A single day within a week template.
/// Can be a training day, rest day, or active rest (cardio only).
/// </summary>
public sealed class WorkoutDay : BaseEntity
{
    public Guid WeekTemplateId { get; private set; }
    public WeekTemplate WeekTemplate { get; private set; } = default!;

    /// <summary>0=Monday … 6=Sunday</summary>
    public int DayOfWeek { get; private set; }
    public DayType DayType { get; private set; }
    public string? Notes { get; private set; }
    public string? Label { get; private set; }  // e.g. "Arms Day", "Push Day"

    private readonly List<WorkoutBlock> _blocks = [];
    public IReadOnlyCollection<WorkoutBlock> Blocks => _blocks.AsReadOnly();

    private WorkoutDay() { }

    internal static WorkoutDay Create(Guid weekTemplateId, int dayOfWeek, DayType dayType, string? notes) =>
        new()
        {
            WeekTemplateId = weekTemplateId,
            DayOfWeek = dayOfWeek,
            DayType = dayType,
            Notes = notes
        };

    public void SetLabel(string label)
    {
        Label = label.Trim();
        Touch();
    }

    public WorkoutBlock AddBlock(WorkoutBlock block)
    {
        // Assign next order
        block.SetOrder(_blocks.Count > 0 ? _blocks.Max(b => b.Order) + 1 : 1);
        _blocks.Add(block);
        Touch();
        return block;
    }

    public Result RemoveBlock(Guid blockId)
    {
        var block = _blocks.FirstOrDefault(b => b.Id == blockId);
        if (block is null)
            return Result.Fail(DomainErrors.WorkoutBlock.NotFound(blockId));

        _blocks.Remove(block);
        ReorderBlocks();
        Touch();
        return Result.Ok();
    }

    private void ReorderBlocks()
    {
        var ordered = _blocks.OrderBy(b => b.Order).ToList();
        for (int i = 0; i < ordered.Count; i++)
            ordered[i].SetOrder(i + 1);
    }

    public void Update(string? notes, string? label, DayType dayType)
    {
        Notes = notes;
        Label = label?.Trim();
        DayType = dayType;
        Touch();
    }
}