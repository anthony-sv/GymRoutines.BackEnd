using FluentAssertions;
using Application.Common.Interfaces;
using Application.Features.Exercises.Commands;
using Application.Features.Exercises.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Caching.Hybrid;
using Moq;

namespace Application.Unit;

public sealed class CreateExerciseHandlerTests
{
    private readonly Mock<IExerciseRepository> _exerciseRepoMock = new();
    private readonly Mock<IEquipmentRepository> _equipmentRepoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<HybridCache> _cacheMock = new();

    private CreateExerciseCommandHandler CreateSut() =>
        new(_exerciseRepoMock.Object, _equipmentRepoMock.Object, _uowMock.Object, _cacheMock.Object);

    [Fact]
    public async Task Handle_NewExercise_ReturnsSuccess()
    {
        // Arrange
        _exerciseRepoMock
            .Setup(x => x.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _uowMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateExerciseCommand(
            Name: "Cable Preacher Curl",
            Type: ExerciseType.Strength,
            Description: "Low cable preacher curl",
            DefaultEquipmentId: null,
            DefaultEquipmentVariantId: null,
            PrimaryMuscles: [MuscleGroup.Biceps],
            SecondaryMuscles: [],
            DefaultCardioType: null);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Cable Preacher Curl");
        result.Value.Type.Should().Be(ExerciseType.Strength);
        result.Value.PrimaryMuscles.Should().Contain(MuscleGroup.Biceps);

        _exerciseRepoMock.Verify(
            x => x.AddAsync(It.Is<Exercise>(e => e.Name == "Cable Preacher Curl"), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateName_ReturnsConflictError()
    {
        // Arrange
        _exerciseRepoMock
            .Setup(x => x.ExistsByNameAsync("Standing Barbell Curl", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateExerciseCommand(
            "Standing Barbell Curl", ExerciseType.Strength, null, null, null,
            [], [], null);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Metadata["ErrorCode"].Should().Be("EXERCISE_NAME_EXISTS");
        _exerciseRepoMock.Verify(
            x => x.AddAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_CardioExerciseWithEquipment_WhenEquipmentNotFound_ReturnsFail()
    {
        // Arrange
        var equipmentId = Guid.NewGuid();

        _exerciseRepoMock
            .Setup(x => x.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _equipmentRepoMock
            .Setup(x => x.GetByIdAsync(equipmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Equipment?)null);

        var command = new CreateExerciseCommand(
            "Custom Exercise", ExerciseType.Strength, null, equipmentId, null,
            [], [], null);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Metadata["ErrorCode"].Should().Be("NOT_FOUND");
    }
}