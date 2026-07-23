using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Species.GetSpecies;
using UFAMS.Application.Interfaces;
using Xunit;
using SpeciesEntity = UFAMS.Domain.Entities.Species;

namespace UFAMS.Application.Tests.SpeciesTests;

public class GetSpeciesHandlerTests
{
    [Fact]
    public async Task GetSpeciesHandler_ReturnsSpecies()
    {
        // Arrange
        var repository =
            new Mock<ISpeciesRepository>();

        repository
            .Setup(x =>
                x.GetAllAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<SpeciesEntity>
                {
                    new SpeciesEntity(
                        "Douglas Fir",
                        "Pseudotsuga menziesii",
                        true)
                });


        var handler =
            new GetSpeciesHandler(
                repository.Object);


        // Act
        var result =
            await handler.Handle(
                new GetSpeciesQuery());


        // Assert
        result.Should()
            .HaveCount(1);

        result[0].CommonName
            .Should()
            .Be("Douglas Fir");

        result[0].ScientificName
            .Should()
            .Be("Pseudotsuga menziesii");

        result[0].IsNative
            .Should()
            .BeTrue();


        repository.Verify(
            x => x.GetAllAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task GetSpeciesHandler_WhenNoSpecies_ReturnsEmptyList()
    {
        // Arrange
        var repository =
            new Mock<ISpeciesRepository>();

        repository
            .Setup(x =>
                x.GetAllAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<SpeciesEntity>());


        var handler =
            new GetSpeciesHandler(
                repository.Object);


        // Act
        var result =
            await handler.Handle(
                new GetSpeciesQuery());


        // Assert
        result.Should()
            .BeEmpty();


        repository.Verify(
            x => x.GetAllAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}