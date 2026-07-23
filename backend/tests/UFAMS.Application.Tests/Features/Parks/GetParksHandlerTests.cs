using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Parks.GetParks;
using UFAMS.Application.Interfaces;
using Xunit;
using ParkEntity = UFAMS.Domain.Entities.Park;

namespace UFAMS.Application.Tests.ParksTests;

public class GetParksHandlerTests
{
    [Fact]
    public async Task GetParksHandler_ReturnsParks()
    {
        // Arrange
        var repository =
            new Mock<IParkRepository>();

        repository
            .Setup(x =>
                x.GetAllAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<ParkEntity>
                {
                    new ParkEntity(
                        "Stanley Park",
                        new UFAMS.Domain.ValueObjects.GeoCoordinate(
                            49.3043,
                            -123.1443),
                        405)
                });


        var handler =
            new GetParksHandler(
                repository.Object);


        // Act
        var result =
            await handler.Handle(
                new GetParksQuery());


        // Assert
        result.Should()
            .HaveCount(1);


        result[0].Name
            .Should()
            .Be("Stanley Park");


        result[0].AreaInHectares
            .Should()
            .Be(405);


        repository.Verify(
            x => x.GetAllAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }



    [Fact]
    public async Task GetParksHandler_WhenNoParks_ReturnsEmptyList()
    {
        // Arrange
        var repository =
            new Mock<IParkRepository>();


        repository
            .Setup(x =>
                x.GetAllAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<ParkEntity>());


        var handler =
            new GetParksHandler(
                repository.Object);


        // Act
        var result =
            await handler.Handle(
                new GetParksQuery());


        // Assert
        result.Should()
            .BeEmpty();


        repository.Verify(
            x => x.GetAllAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}