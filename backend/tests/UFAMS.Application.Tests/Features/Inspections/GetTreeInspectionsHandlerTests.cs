using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Inspections.GetTreeInspections;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Application.Tests.Common;

namespace UFAMS.Application.Tests.Features.Inspections.GetTreeInspections;

public class GetTreeInspectionsHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingInspections_ReturnsInspectionList()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();


        var inspections =
            new List<Inspection>
            {
                new Inspection(
                    tree.Id,
                    new DateOnly(2025, 1, 1),
                    TreeHealthStatus.Good,
                    "First inspection",
                    "Monitor tree",
                    new DateOnly(2026, 1, 1)),

                new Inspection(
                    tree.Id,
                    new DateOnly(2025, 6, 1),
                    TreeHealthStatus.Fair,
                    "Second inspection",
                    "Schedule pruning",
                    new DateOnly(2026, 6, 1))
            };


        var repository =
            new Mock<IInspectionRepository>();

        repository
            .Setup(r => r.GetByTreeIdAsync(
                tree.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inspections);


        var handler =
            new GetTreeInspectionsHandler(
                repository.Object);


        var query =
            new GetTreeInspectionsQuery(
                tree.Id);


        // Act
        var result =
            await handler.Handle(query);


        // Assert
        result.Should()
            .HaveCount(2);


        result[0].TreeId
            .Should()
            .Be(tree.Id);


        result[0].Notes
            .Should()
            .Be("First inspection");


        result[1].ObservedHealth
            .Should()
            .Be(TreeHealthStatus.Fair);


        result[1].Recommendation
            .Should()
            .Be("Schedule pruning");
    }


    [Fact]
    public async Task Handle_WhenNoInspectionsExist_ReturnsEmptyList()
    {
        // Arrange
        var treeId =
            Guid.NewGuid();


        var repository =
            new Mock<IInspectionRepository>();


        repository
            .Setup(r => r.GetByTreeIdAsync(
                treeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Inspection>());


        var handler =
            new GetTreeInspectionsHandler(
                repository.Object);


        var query =
            new GetTreeInspectionsQuery(
                treeId);


        // Act
        var result =
            await handler.Handle(query);


        // Assert
        result.Should()
            .BeEmpty();
    }
}