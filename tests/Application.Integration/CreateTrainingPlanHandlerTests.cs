using FluentAssertions;
using Application.Common.Interfaces;
using Application.Features.TrainingPlans.Commands;
using Application.Features.TrainingPlans.DTOs;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace Application.Integration;

public sealed class CreateTrainingPlanHandlerTests
{
    private readonly Mock<ITrainingPlanRepository> _planRepoMock = new();
    private readonly Mock<ICurrentUserService> _currentUserMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();

    private CreateTrainingPlanCommandHandler CreateSut() =>
        new(_planRepoMock.Object, _currentUserMock.Object, _uowMock.Object);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsCreatedPlan()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserMock.Setup(x => x.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(x => x.UserId).Returns(userId);
        _uowMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateTrainingPlanCommand(
            Name: "8-Week Arm Focus",
            Description: "Arm hypertrophy program",
            TotalWeeks: 8,
            StructureType: ProgramStructureType.RepeatingWeek,
            IsPublic: false);

        var sut = CreateSut();

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("8-Week Arm Focus");
        result.Value.TotalWeeks.Should().Be(8);
        result.Value.OwnerId.Should().Be(userId);
        result.Value.IsPublic.Should().BeFalse();

        _planRepoMock.Verify(x => x.AddAsync(
            It.Is<TrainingPlan>(p => p.Name == "8-Week Arm Focus" && p.OwnerId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ReturnsUnauthorized()
    {
        // Arrange
        _currentUserMock.Setup(x => x.IsAuthenticated).Returns(false);
        _currentUserMock.Setup(x => x.UserId).Returns((Guid?)null);

        var command = new CreateTrainingPlanCommand("Plan", null, 4, ProgramStructureType.RepeatingWeek);
        var sut = CreateSut();

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Metadata["ErrorCode"].Should().Be("UNAUTHORIZED");
        _planRepoMock.Verify(x => x.AddAsync(It.IsAny<TrainingPlan>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void TrainingPlan_Create_WithInvalidWeeks_ReturnsFail(int weeks)
    {
        // Arrange & Act
        var result = TrainingPlan.Create("Plan", null, weeks, ProgramStructureType.RepeatingWeek, Guid.NewGuid());

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message.Contains("at least 1"));
    }
}