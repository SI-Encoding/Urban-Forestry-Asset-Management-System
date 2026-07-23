using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Trees.FindNearbyTrees;
using UFAMS.Application.Interfaces;
using UFAMS.Application.Tests.Common;
using UFAMS.Domain.Entities;

namespace UFAMS.Application.Tests.Features.Trees.FindNearbyTrees;

public class FindNearbyTreesHandlerTests
{
    [Fact]
    public async Task Handle_WhenTreesWithinRadius_ReturnsTrees()
    {
        // Arrange
        var trees = new List<Tree>
        {
            TestDataFactory.CreateTree()
        };

        var repository =
            new Mock<ITreeRepository>();

        repository
            .Setup(r => r.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(trees);

        var handler =
            new FindNearbyTreesHandler(
                repository.Object);

        var query =
            new FindNearbyTreesQuery(
                49.3043,
                -123.1443,
                1000);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Should()
            .HaveCount(1);

        result[0].AssetTag
            .Should()
            .Be("TREE-001");

        result[0].DistanceMeters
            .Should()
            .Be(0);
    }


    [Fact]
    public async Task Handle_WhenTreeOutsideRadius_ReturnsEmptyList()
    {
        // Arrange
        var trees = new List<Tree>
        {
            TestDataFactory.CreateTree()
        };

        var repository =
            new Mock<ITreeRepository>();

        repository
            .Setup(r => r.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(trees);

        var handler =
            new FindNearbyTreesHandler(
                repository.Object);

        var query =
            new FindNearbyTreesQuery(
                0,
                0,
                10);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Should()
            .BeEmpty();
    }


    [Fact]
    public async Task Handle_ReturnsTreesOrderedByDistance()
    {
        // Arrange
        var closeTree =
            TestDataFactory.CreateTree(
                assetTag: "TREE-CLOSE",
                latitude: 49.3045,
                longitude: -123.1445);

        var farTree =
            TestDataFactory.CreateTree(
                assetTag: "TREE-FAR",
                latitude: 49.3500,
                longitude: -123.2000);

        var repository =
            new Mock<ITreeRepository>();

        repository
            .Setup(r => r.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Tree>
            {
                farTree,
                closeTree
            });

        var handler =
            new FindNearbyTreesHandler(
                repository.Object);

        var query =
            new FindNearbyTreesQuery(
                49.3043,
                -123.1443,
                10000);

        // Act
        var result =
            await handler.Handle(query);

        // Assert
        result.Should()
            .HaveCount(2);

        result[0].AssetTag
            .Should()
            .Be("TREE-CLOSE");

        result[1].AssetTag
            .Should()
            .Be("TREE-FAR");
    }


    [Fact]
    public async Task Handle_WhenQueryInvalid_ThrowsArgumentException()
    {
        // Arrange
        var repository =
            new Mock<ITreeRepository>();

        var handler =
            new FindNearbyTreesHandler(
                repository.Object);

        var query =
            new FindNearbyTreesQuery(
                100,
                -123.1443,
                1000);

        // Act
        Func<Task> action = () =>
            handler.Handle(query);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentException>();
    }
}