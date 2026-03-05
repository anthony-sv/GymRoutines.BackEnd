using FluentAssertions;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Unit;

public sealed class ExerciseTests
{
    [Fact]
    public void Exercise_Create_StrengthType_HasCorrectProperties()
    {
        // Arrange & Act
        var exercise = Exercise.Create(
            "Barbell Curl",
            ExerciseType.Strength,
            "Classic curl",
            primaryMuscles: [MuscleGroup.Biceps],
            secondaryMuscles: [MuscleGroup.Forearms]);

        // Assert
        exercise.Name.Should().Be("Barbell Curl");
        exercise.Type.Should().Be(ExerciseType.Strength);
        exercise.PrimaryMuscles.Should().Contain(MuscleGroup.Biceps);
        exercise.SecondaryMuscles.Should().Contain(MuscleGroup.Forearms);
        exercise.IsSeeded.Should().BeFalse();
        exercise.DefaultCardioType.Should().BeNull();
    }

    [Fact]
    public void Exercise_Create_TimedType_ForPoseOrFlex()
    {
        // Act
        var exercise = Exercise.Create(
            "Front Double Biceps Flex",
            ExerciseType.Timed,
            "Isometric pose hold",
            primaryMuscles: [MuscleGroup.Biceps],
            secondaryMuscles: [MuscleGroup.Shoulders]);

        // Assert
        exercise.Type.Should().Be(ExerciseType.Timed);
        exercise.DefaultCardioType.Should().BeNull();
    }

    [Fact]
    public void StandardSetBlock_Create_WithDropSet_HasCorrectConfiguration()
    {
        // Arrange & Act
        var block = StandardSetBlock.Create(
            workoutDayId: Guid.NewGuid(),
            exerciseId: Guid.NewGuid(),
            sets: 3,
            restSeconds: 120,
            minReps: 10,
            maxReps: 15,
            intensityTechnique: IntensityTechnique.DropSet,
            dropSetScope: DropSetScope.LastSetOnly,
            dropCount: 2);

        // Assert
        block.IntensityTechnique.Should().Be(IntensityTechnique.DropSet);
        block.DropSetScope.Should().Be(DropSetScope.LastSetOnly);
        block.DropCount.Should().Be(2);
        block.Sets.Should().Be(3);
        block.MinReps.Should().Be(10);
        block.MaxReps.Should().Be(15);
    }

    [Fact]
    public void CircuitBlock_AddExercise_AssignsOrderIncrementally()
    {
        // Arrange
        var circuit = CircuitBlock.Create(Guid.NewGuid(), rounds: 7, restBetweenRoundsSeconds: 30);

        // Act
        circuit.AddExercise(Guid.NewGuid(), durationSeconds: null, minReps: null, maxReps: null);
        circuit.AddExercise(Guid.NewGuid(), durationSeconds: 10); // pose/flex
        circuit.AddExercise(Guid.NewGuid(), durationSeconds: 30); // rest/hold

        // Assert
        circuit.Exercises.Should().HaveCount(3);
        circuit.Exercises.OrderBy(e => e.Order).Select(e => e.Order).Should().BeEquivalentTo([1, 2, 3]);
        circuit.Rounds.Should().Be(7);
        circuit.RestBetweenRoundsSeconds.Should().Be(30);
    }

    [Fact]
    public void CardioBlock_Create_HasCorrectMachineSettings()
    {
        // Act
        var block = CardioBlock.Create(
            workoutDayId: Guid.NewGuid(),
            cardioType: CardioType.Walking,
            durationMinutes: 30,
            speed: 5.0m,
            incline: 10.0m,
            resistance: null,
            targetCalories: 250,
            targetHeartRateBpm: 130);

        // Assert
        block.CardioType.Should().Be(CardioType.Walking);
        block.DurationMinutes.Should().Be(30);
        block.Speed.Should().Be(5.0m);
        block.Incline.Should().Be(10.0m);
        block.TargetCalories.Should().Be(250);
        block.TargetHeartRateBpm.Should().Be(130);
    }
}