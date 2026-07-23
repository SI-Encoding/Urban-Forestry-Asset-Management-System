using FluentAssertions;
using Moq;
using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Features.Inspections.GetInspection;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.Inspections.GetInspection;

public class GetInspectionHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingInspection_ReturnsInspectionResponse()
    {
        // Arrange
        var inspection =
            CreateInspection();

        var repository =
            new Mock<IInspectionRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                inspection.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inspection);


        var handler =
            new GetInspectionHandler(
                repository.Object);


        var query =
            new GetInspectionQuery(
                inspection.Id);


        // Act
        var result =
            await handler.Handle(query);


        // Assert
        result.Id
            .Should()
            .Be(inspection.Id);

        result.TreeId
            .Should()
            .Be(inspection.TreeId);

        result.ObservedHealth
            .Should()
            .Be(TreeHealthStatus.Good);

        result.Notes
            .Should()
            .Be("Tree inspected.");

        result.Recommendation
            .Should()
            .Be("Continue monitoring.");
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


        var handler =
            new GetInspectionHandler(
                repository.Object);


        var query =
            new GetInspectionQuery(
                inspectionId);


        // Act
        Func<Task> action = async () =>
            await handler.Handle(query);


        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>();
    }


    private static Inspection CreateInspection()
    {
        var tree =
            UFAMS.Application.Tests.Common.TestDataFactory
                .CreateTree();


        return new Inspection(
            tree.Id,
            new DateOnly(2025, 1, 1),
            TreeHealthStatus.Good,
            "Tree inspected.",
            "Continue monitoring.",
            new DateOnly(2026, 1, 1));
    }
}