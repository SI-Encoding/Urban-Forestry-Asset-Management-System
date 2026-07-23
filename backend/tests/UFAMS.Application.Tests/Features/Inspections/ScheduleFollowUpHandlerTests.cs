using FluentAssertions;
using Moq;
using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Features.Inspections.ScheduleFollowUp;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.Inspections.ScheduleFollowUp;

public class ScheduleFollowUpHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingInspection_UpdatesFollowUpDate()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();

        var inspection =
            new Inspection(
                tree.Id,
                new DateOnly(2025, 1, 1),
                TreeHealthStatus.Good,
                "Inspection notes",
                "Monitor tree",
                null);


        var repository =
            new Mock<IInspectionRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                inspection.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inspection);


        var unitOfWork =
            new Mock<IUnitOfWork>();


        var handler =
            new ScheduleFollowUpHandler(
                repository.Object,
                unitOfWork.Object);


        var nextInspectionDate =
            new DateOnly(2026, 1, 1);


        var command =
            new ScheduleFollowUpCommand(
                nextInspectionDate);


        // Act
        var result =
            await handler.Handle(
                inspection.Id,
                command);


        // Assert
        result.Id
            .Should()
            .Be(inspection.Id);

        result.TreeId
            .Should()
            .Be(tree.Id);

        result.NextInspectionDate
            .Should()
            .Be(nextInspectionDate);


        inspection.NextInspectionDate
            .Should()
            .Be(nextInspectionDate);


        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task Handle_WhenInspectionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var inspectionId =
            Guid.NewGuid();


        var repository =
            new Mock<IInspectionRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                inspectionId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Inspection?)null);


        var unitOfWork =
            new Mock<IUnitOfWork>();


        var handler =
            new ScheduleFollowUpHandler(
                repository.Object,
                unitOfWork.Object);


        var command =
            new ScheduleFollowUpCommand(
                new DateOnly(2026, 1, 1));


        // Act
        Func<Task> action = async () =>
            await handler.Handle(
                inspectionId,
                command);


        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>();


        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}