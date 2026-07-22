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

    private static Tree CreateTree(
        string assetTag,
        string speciesName,
        string parkName)
    {
        var species =
            new Species(
                speciesName,
                speciesName,
                true);

        var park =
            new Park(
                parkName,
                new GeoCoordinate(
                    49.0,
                    -123.0),
                100);

        return new Tree(
            assetTag,
            species,
            park,
            new GeoCoordinate(
                49.1,
                -123.1),
            TreeHealthStatus.Good,
            new DateOnly(2020, 1, 1),
            12,
            30);
    }
}