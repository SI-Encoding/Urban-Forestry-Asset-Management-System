using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.Inspections.CreateInspection;

public class CreateInspectionHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_CreatesInspection()
    {
        // Arrange
        var tree =
            TestDataFactory.CreateTree();

        var treeRepository =
            new Mock<ITreeRepository>();

        var inspectionRepository =
            new Mock<IInspectionRepository>();

        var unitOfWork =
            new Mock<IUnitOfWork>();

        treeRepository
            .Setup(r => r.GetByIdAsync(
                tree.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tree);


        var handler =
            new CreateInspectionHandler(
                treeRepository.Object,
                inspectionRepository.Object,
                unitOfWork.Object);


        var command =
            new CreateInspectionCommand(
                new DateOnly(2025, 1, 1),
                TreeHealthStatus.Good,
                "Tree inspection completed.",
                "Continue monitoring tree health.",
                new DateOnly(2026, 1, 1));


        // Act
        var result =
            await handler.Handle(
                tree.Id,
                command);


        // Assert
        result.TreeId
            .Should()
            .Be(tree.Id);

        result.ObservedHealth
            .Should()
            .Be(TreeHealthStatus.Good);

        result.Notes
            .Should()
            .Be("Tree inspection completed.");

        result.Recommendation
            .Should()
            .Be("Continue monitoring tree health.");


        inspectionRepository.Verify(r =>
            r.AddAsync(
                It.IsAny<UFAMS.Domain.Entities.Inspection>(),
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
        var treeRepository =
            new Mock<ITreeRepository>();

        var inspectionRepository =
            new Mock<IInspectionRepository>();

        var unitOfWork =
            new Mock<IUnitOfWork>();

        var treeId =
            Guid.NewGuid();


        treeRepository
            .Setup(r => r.GetByIdAsync(
                treeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UFAMS.Domain.Entities.Tree?)null);


        var handler =
            new CreateInspectionHandler(
                treeRepository.Object,
                inspectionRepository.Object,
                unitOfWork.Object);


        var command =
            new CreateInspectionCommand(
                new DateOnly(2025, 1, 1),
                TreeHealthStatus.Good,
                "Notes",
                "Recommendation",
                null);


        // Act
        Func<Task> action = async () =>
            await handler.Handle(
                treeId,
                command);


        // Assert
        await action.Should()
            .ThrowAsync<UFAMS.Application.Common.Exceptions.NotFoundException>();


        inspectionRepository.Verify(r =>
            r.AddAsync(
                It.IsAny<UFAMS.Domain.Entities.Inspection>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

}