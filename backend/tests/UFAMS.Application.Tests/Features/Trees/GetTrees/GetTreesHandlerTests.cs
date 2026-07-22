using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Tests.Features.Trees.GetTrees;

public class GetTreesHandlerTests
{
    [Fact]
    public async Task Handle_WithNoFilters_ReturnsTrees()
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
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(trees);

        var handler =
            new GetTreesHandler(repository.Object);

        var query =
            new GetTreesQuery(
                null,
                null,
                null);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Should()
            .HaveCount(2);

        result[0].AssetTag
            .Should()
            .Be("TREE-001");

        result[0].SpeciesName
            .Should()
            .Be("Douglas Fir");

        result[0].ParkName
            .Should()
            .Be("Stanley Park");
    }


    [Fact]
    public async Task Handle_WithFilters_PassesFiltersToRepository()
    {
        // Arrange
        var repository =
            new Mock<ITreeRepository>();

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
            new GetTreesHandler(repository.Object);

        var parkId =
            Guid.NewGuid();

        var query =
            new GetTreesQuery(
                parkId,
                null,
                TreeHealthStatus.Good);

        // Act
        await handler.Handle(query);

        // Assert
        repository.Verify(r =>
            r.SearchAsync(
                parkId,
                null,
                TreeHealthStatus.Good,
                null,
                null,
                null,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task Handle_WhenRepositoryReturnsNoTrees_ReturnsEmptyList()
    {
        // Arrange
        var repository =
            new Mock<ITreeRepository>();

        repository
            .Setup(r => r.SearchAsync(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Tree>());

        var handler =
            new GetTreesHandler(repository.Object);

        var query =
            new GetTreesQuery(
                null,
                null,
                null);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Should()
            .BeEmpty();
    }
}