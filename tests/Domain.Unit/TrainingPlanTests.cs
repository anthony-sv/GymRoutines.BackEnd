using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.Unit;

public sealed class TrainingPlanTests
{
    [Fact]
    public void TrainingPlan_AddWeekTemplate_RepeatingPlan_SecondWeekFails()
    {
        // Arrange
        var planResult = TrainingPlan.Create("Plan", null, 8, ProgramStructureType.RepeatingWeek, Guid.NewGuid());
        var plan = planResult.Value;

        var firstWeekResult = plan.AddWeekTemplate(1);
        firstWeekResult.IsSuccess.Should().BeTrue();

        // Act — try to add second week to a repeating plan
        var secondWeekResult = plan.AddWeekTemplate(2);

        // Assert
        secondWeekResult.IsFailed.Should().BeTrue();
        secondWeekResult.Errors.First().Message.Should().Contain("one week template");
    }

    [Fact]
    public void TrainingPlan_AddWeekTemplate_FullSchedule_AddsBeyondTotalWeeks_Fails()
    {
        // Arrange
        var planResult = TrainingPlan.Create("Plan", null, 2, ProgramStructureType.FullSchedule, Guid.NewGuid());
        var plan = planResult.Value;

        plan.AddWeekTemplate(1).IsSuccess.Should().BeTrue();
        plan.AddWeekTemplate(2).IsSuccess.Should().BeTrue();

        // Act — try to add a 3rd week beyond TotalWeeks
        var result = plan.AddWeekTemplate(3);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Message.Should().Contain("2 week templates");
    }

    [Fact]
    public void WeekTemplate_AddDay_DuplicateDayOfWeek_Fails()
    {
        // Arrange
        var planResult = TrainingPlan.Create("Plan", null, 4, ProgramStructureType.RepeatingWeek, Guid.NewGuid());
        var plan = planResult.Value;
        var week = plan.AddWeekTemplate(1).Value;

        week.AddDay(0, DayType.Training).IsSuccess.Should().BeTrue(); // Monday

        // Act — add Monday again
        var result = week.AddDay(0, DayType.Rest);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void WorkoutDay_AddBlock_SetsOrderCorrectly()
    {
        // Arrange
        var planResult = TrainingPlan.Create("Plan", null, 4, ProgramStructureType.RepeatingWeek, Guid.NewGuid());
        var plan = planResult.Value;
        var week = plan.AddWeekTemplate(1).Value;
        var day = week.AddDay(0, DayType.Training).Value;

        var exerciseId = Guid.NewGuid();
        var workoutDayId = day.Id;

        var block1 = StandardSetBlock.Create(workoutDayId, exerciseId, 3, 120, 10, 15);
        var block2 = StandardSetBlock.Create(workoutDayId, exerciseId, 2, 90, 8, 12);

        // Act
        day.AddBlock(block1);
        day.AddBlock(block2);

        // Assert
        block1.Order.Should().Be(1);
        block2.Order.Should().Be(2);
        day.Blocks.Should().HaveCount(2);
    }

    [Fact]
    public void WorkoutDay_RemoveBlock_ReordersRemainingBlocks()
    {
        // Arrange
        var planResult = TrainingPlan.Create("Plan", null, 4, ProgramStructureType.RepeatingWeek, Guid.NewGuid());
        var plan = planResult.Value;
        var week = plan.AddWeekTemplate(1).Value;
        var day = week.AddDay(0, DayType.Training).Value;

        var workoutDayId = day.Id;
        var exerciseId = Guid.NewGuid();

        var block1 = StandardSetBlock.Create(workoutDayId, exerciseId, 3, 120, 10, 15);
        var block2 = StandardSetBlock.Create(workoutDayId, exerciseId, 2, 90, 8, 12);
        var block3 = StandardSetBlock.Create(workoutDayId, exerciseId, 2, 60, 12, 15);

        day.AddBlock(block1);
        day.AddBlock(block2);
        day.AddBlock(block3);

        // Act — remove block 2 (order 2)
        var removeResult = day.RemoveBlock(block2.Id);

        // Assert
        removeResult.IsSuccess.Should().BeTrue();
        day.Blocks.Should().HaveCount(2);
        day.Blocks.OrderBy(b => b.Order).First().Order.Should().Be(1);
        day.Blocks.OrderBy(b => b.Order).Last().Order.Should().Be(2);
    }
}