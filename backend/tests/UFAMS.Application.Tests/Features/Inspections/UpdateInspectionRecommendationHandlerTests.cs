using FluentAssertions;
using Moq;
using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Features.Inspections.UpdateRecommendation;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.Inspections.UpdateRecommendation;

public class UpdateInspectionRecommendationHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingInspection_UpdatesRecommendation()
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
                "Old recommendation",
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
            new UpdateInspectionRecommendationHandler(
                repository.Object,
                unitOfWork.Object);


        var command =
            new UpdateInspectionRecommendationCommand(
                "Prune branches next season");


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

        result.Recommendation
            .Should()
            .Be("Prune branches next season");


        inspection.Recommendation
            .Should()
            .Be("Prune branches next season");


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
            new UpdateInspectionRecommendationHandler(
                repository.Object,
                unitOfWork.Object);


        var command =
            new UpdateInspectionRecommendationCommand(
                "New recommendation");


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