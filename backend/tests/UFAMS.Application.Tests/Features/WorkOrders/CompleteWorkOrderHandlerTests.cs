using FluentAssertions;
using Moq;
using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Features.WorkOrders.CompleteWorkOrder;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.WorkOrders.CompleteWorkOrder;

public class CompleteWorkOrderHandlerTests
{
    [Fact]
    public async Task Handle_WithInProgressWorkOrder_CompletesWorkOrder()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();

        var employee =
            new Employee(
                "John Smith",
                "Arborist");

        var workOrder =
            new WorkOrder(
                tree,
                "Prune branches",
                DateOnly.FromDateTime(
                    DateTime.UtcNow.AddDays(30)));

        workOrder.AssignEmployee(employee);
        workOrder.Start();

        var repository =
            new Mock<IWorkOrderRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                workOrder.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workOrder);

        var unitOfWork =
            new Mock<IUnitOfWork>();

        var handler =
            new CompleteWorkOrderHandler(
                repository.Object,
                unitOfWork.Object);

        // Act
        var result =
            await handler.Handle(workOrder.Id);

        // Assert
        result.Id.Should().Be(workOrder.Id);

        result.Status.Should()
            .Be(WorkOrderStatus.Completed);

        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenWorkOrderDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var repository =
            new Mock<IWorkOrderRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((WorkOrder?)null);

        var unitOfWork =
            new Mock<IUnitOfWork>();

        var handler =
            new CompleteWorkOrderHandler(
                repository.Object,
                unitOfWork.Object);

        // Act
        Func<Task> action = async () =>
            await handler.Handle(Guid.NewGuid());

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>();

        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenWorkOrderIsNotInProgress_ThrowsInvalidOperationException()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();

        var workOrder =
            new WorkOrder(
                tree,
                "Prune branches",
                DateOnly.FromDateTime(
                    DateTime.UtcNow.AddDays(30)));

        var repository =
            new Mock<IWorkOrderRepository>();

        repository
            .Setup(r => r.GetByIdAsync(
                workOrder.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workOrder);

        var unitOfWork =
            new Mock<IUnitOfWork>();

        var handler =
            new CompleteWorkOrderHandler(
                repository.Object,
                unitOfWork.Object);

        // Act
        Func<Task> action = async () =>
            await handler.Handle(workOrder.Id);

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>();

        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}