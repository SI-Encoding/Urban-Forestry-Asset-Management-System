using FluentAssertions;
using Moq;
using UFAMS.Application.Features.WorkOrders.GetTreeWorkOrders;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;

namespace UFAMS.Application.Tests.Features.WorkOrders.GetTreeWorkOrders;

public class GetTreeWorkOrdersHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingWorkOrders_ReturnsWorkOrders()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();

        var dueDate =
            DateOnly.FromDateTime(
                DateTime.UtcNow.AddDays(30));

        var workOrders =
            new List<WorkOrder>
            {
                new(
                    tree,
                    "Prune branches",
                    dueDate),

                new(
                    tree,
                    "Inspect root system",
                    dueDate.AddDays(7))
            };

        var repository =
            new Mock<IWorkOrderRepository>();

        repository
            .Setup(r => r.GetByTreeIdAsync(
                tree.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workOrders);

        var handler =
            new GetTreeWorkOrdersHandler(
                repository.Object);

        var query =
            new GetTreeWorkOrdersQuery(
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

        result[0].Description
            .Should()
            .Be("Prune branches");

        result[0].Status
            .Should()
            .Be(workOrders[0].Status);

        result[1].Description
            .Should()
            .Be("Inspect root system");

        result[1].Status
            .Should()
            .Be(workOrders[1].Status);
    }

    [Fact]
    public async Task Handle_WithNoWorkOrders_ReturnsEmptyList()
    {
        // Arrange
        var treeId =
            Guid.NewGuid();

        var repository =
            new Mock<IWorkOrderRepository>();

        repository
            .Setup(r => r.GetByTreeIdAsync(
                treeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<WorkOrder>());

        var handler =
            new GetTreeWorkOrdersHandler(
                repository.Object);

        // Act
        var result =
            await handler.Handle(
                new GetTreeWorkOrdersQuery(treeId));

        // Assert
        result.Should().BeEmpty();
    }
}