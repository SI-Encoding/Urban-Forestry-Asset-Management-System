using FluentAssertions;
using Moq;
using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Features.Parks.GetParkInventory;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;
using Xunit;
using ParkEntity = UFAMS.Domain.Entities.Park;
using SpeciesEntity = UFAMS.Domain.Entities.Species;
using TreeEntity = UFAMS.Domain.Entities.Tree;

namespace UFAMS.Application.Tests.ParksTests;

public class GetParkInventoryHandlerTests
{
    [Fact]
    public async Task GetParkInventoryHandler_ReturnsInventory()
    {
        // Arrange
        var parkRepository =
            new Mock<IParkRepository>();

        var treeRepository =
            new Mock<ITreeRepository>();


        var species =
            new SpeciesEntity(
                "Douglas Fir",
                "Pseudotsuga menziesii",
                true);


        var park =
            new ParkEntity(
                "Stanley Park",
                new GeoCoordinate(
                    49.3043,
                    -123.1443),
                405);


        var tree =
            new TreeEntity(
                "TREE-001",
                species,
                park,
                new GeoCoordinate(
                    49.3043,
                    -123.1443),
                TreeHealthStatus.Good,
                new DateOnly(2020, 1, 1),
                12,
                30);


        parkRepository
            .Setup(x =>
                x.GetByIdAsync(
                    park.Id,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(park);


        treeRepository
            .Setup(x =>
                x.GetByParkIdAsync(
                    park.Id,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<TreeEntity>
                {
                    tree
                });


        var handler =
            new GetParkInventoryHandler(
                parkRepository.Object,
                treeRepository.Object);


        // Act
        var result =
            await handler.Handle(
                new GetParkInventoryQuery(
                    park.Id));


        // Assert
        result.ParkName
            .Should()
            .Be("Stanley Park");


        result.TotalTrees
            .Should()
            .Be(1);


        result.HealthyTrees
            .Should()
            .Be(1);


        result.TreesNeedingAttention
            .Should()
            .Be(0);


        result.Species
            .Should()
            .ContainSingle();


        result.Species[0].CommonName
            .Should()
            .Be("Douglas Fir");
    }



    [Fact]
    public async Task GetParkInventoryHandler_WhenParkDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var parkRepository =
            new Mock<IParkRepository>();

        var treeRepository =
            new Mock<ITreeRepository>();


        parkRepository
            .Setup(x =>
                x.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                (ParkEntity?)null);


        var handler =
            new GetParkInventoryHandler(
                parkRepository.Object,
                treeRepository.Object);


        // Act
        var action =
            async () =>
                await handler.Handle(
                    new GetParkInventoryQuery(
                        Guid.NewGuid()));


        // Assert
        await action
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}