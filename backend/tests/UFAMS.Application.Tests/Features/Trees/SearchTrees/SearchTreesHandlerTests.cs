using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Trees.SearchTrees;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Tests.Features.Trees.SearchTrees;

public class SearchTreesHandlerTests
{
    [Fact]
    public async Task Handle_WithNoFilters_ReturnsAllTrees()
    {
        // Arrange
        var trees = new List<Tree>
        {
            TestDataFactory.CreateTree(),

            TestDataFactory.CreateTree(
                assetTag: "TREE-002",
                speciesName: "Red Maple",
                parkName: "Queen Elizabeth Park")
        };

        var repository = new Mock<ITreeRepository>();

        repository
            .Setup(r => r.SearchAsync(
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<TreeHealthStatus?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(trees);

        var handler =
            new SearchTreesHandler(repository.Object);

        var query =
            new SearchTreesQuery(
                null,
                null,
                null,
                null,
                null,
                null,
                null);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Should().HaveCount(2);

        result[0].AssetTag.Should().Be("TREE-001");

        result[1].AssetTag.Should().Be("TREE-002");
    }

    [Fact]
    public async Task Handle_WithFilters_PassesFiltersToRepository()
    {
        // Arrange
        var repository = new Mock<ITreeRepository>();

        repository
            .Setup(r => r.SearchAsync(
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<TreeHealthStatus?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Tree>());

        var handler =
            new SearchTreesHandler(repository.Object);

        var parkId = Guid.NewGuid();

        var query =
            new SearchTreesQuery(
                parkId,
                null,
                TreeHealthStatus.Good,
                49.0,
                50.0,
                -124.0,
                -123.0);

        // Act
        await handler.Handle(query);

        // Assert
        repository.Verify(r =>
            r.SearchAsync(
                parkId,
                null,
                TreeHealthStatus.Good,
                49.0,
                50.0,
                -124.0,
                -123.0,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsNoTrees_ReturnsEmptyList()
    {
        // Arrange
        var repository = new Mock<ITreeRepository>();

        repository
            .Setup(r => r.SearchAsync(
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<TreeHealthStatus?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Tree>());

        var handler =
            new SearchTreesHandler(repository.Object);

        var query =
            new SearchTreesQuery(
                null,
                null,
                null,
                null,
                null,
                null,
                null);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Should().BeEmpty();
    }
}