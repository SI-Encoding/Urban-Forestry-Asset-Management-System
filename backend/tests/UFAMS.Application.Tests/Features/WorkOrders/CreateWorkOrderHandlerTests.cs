using FluentAssertions;
using Moq;
using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.WorkOrders.CreateWorkOrder;

public class CreateWorkOrderHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingTree_CreatesWorkOrder()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();


        var treeRepository =
            new Mock<ITreeRepository>();

        treeRepository
            .Setup(r => r.GetByIdAsync(
                tree.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tree);


        var workOrderRepository =
            new Mock<IWorkOrderRepository>();

        var unitOfWork =
            new Mock<IUnitOfWork>();


        var handler =
            new CreateWorkOrderHandler(
                treeRepository.Object,
                workOrderRepository.Object,
                unitOfWork.Object);


        var command =
            new CreateWorkOrderCommand(
                "Remove damaged branches",
                DateOnly.FromDateTime(
                    DateTime.UtcNow.AddDays(30)));


        // Act
        var result =
            await handler.Handle(
                tree.Id,
                command);


        // Assert
        result.TreeId
            .Should()
            .Be(tree.Id);


        result.Description
            .Should()
            .Be("Remove damaged branches");


        result.Status
            .Should()
            .Be(WorkOrderStatus.Open);


        result.DueDate
            .Should()
            .Be(command.DueDate);


        workOrderRepository.Verify(r =>
            r.AddAsync(
                It.IsAny<UFAMS.Domain.Entities.WorkOrder>(),
                It.IsAny<CancellationToken>()),
            Times.Once);


        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task Handle_WhenTreeDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var treeId =
            Guid.NewGuid();


        var treeRepository =
            new Mock<ITreeRepository>();

        treeRepository
            .Setup(r => r.GetByIdAsync(
                treeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UFAMS.Domain.Entities.Tree?)null);


        var workOrderRepository =
            new Mock<IWorkOrderRepository>();


        var unitOfWork =
            new Mock<IUnitOfWork>();


        var handler =
            new CreateWorkOrderHandler(
                treeRepository.Object,
                workOrderRepository.Object,
                unitOfWork.Object);


        var command =
            new CreateWorkOrderCommand(
                "Remove damaged branches",
                null);


        // Act
        Func<Task> action = async () =>
            await handler.Handle(
                treeId,
                command);


        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>();


        workOrderRepository.Verify(r =>
            r.AddAsync(
                It.IsAny<UFAMS.Domain.Entities.WorkOrder>(),
                It.IsAny<CancellationToken>()),
            Times.Never);


        unitOfWork.Verify(u =>
            u.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}