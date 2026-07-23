using FluentAssertions;
using Moq;
using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Features.WorkOrders.GetWorkOrder;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;

namespace UFAMS.Application.Tests.Features.WorkOrders.GetWorkOrder;

public class GetWorkOrderHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingWorkOrder_ReturnsWorkOrder()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();

        var dueDate =
            DateOnly.FromDateTime(
                DateTime.UtcNow.AddDays(30));

        var workOrder =
            new WorkOrder(
                tree,
                "Remove damaged branches",
                dueDate);

        var repository =
            new Mock<IWorkOrderRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                workOrder.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workOrder);

        var handler =
            new GetWorkOrderHandler(
                repository.Object);

        var query =
            new GetWorkOrderQuery(
                workOrder.Id);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Id
            .Should()
            .Be(workOrder.Id);

        result.TreeId
            .Should()
            .Be(tree.Id);

        result.Description
            .Should()
            .Be("Remove damaged branches");

        result.Status
            .Should()
            .Be(workOrder.Status);

        result.CreatedDate
            .Should()
            .Be(workOrder.CreatedDate);

        result.DueDate
            .Should()
            .Be(dueDate);

        result.CompletedDate
            .Should()
            .BeNull();
    }


    [Fact]
    public async Task Handle_WhenWorkOrderDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var workOrderId =
            Guid.NewGuid();

        var repository =
            new Mock<IWorkOrderRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                workOrderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((WorkOrder?)null);

        var handler =
            new GetWorkOrderHandler(
                repository.Object);

        var query =
            new GetWorkOrderQuery(
                workOrderId);

        // Act
        Func<Task> action = async () =>
            await handler.Handle(query);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>();
    }
}