using FluentAssertions;
using Moq;
using UFAMS.Application.Features.Species.SearchSpecies;
using UFAMS.Application.Interfaces;
using Xunit;
using SpeciesEntity = UFAMS.Domain.Entities.Species;

namespace UFAMS.Application.Tests.SpeciesTests;

public class SearchSpeciesHandlerTests
{
    [Fact]
    public async Task SearchSpeciesHandler_WithMatchingName_ReturnsSpecies()
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
                        true),

                    new SpeciesEntity(
                        "Red Maple",
                        "Acer rubrum",
                        false)
                });


        var handler =
            new SearchSpeciesHandler(
                repository.Object);


        // Act
        var result =
            await handler.Handle(
                new SearchSpeciesQuery("fir"));


        // Assert
        result.Should()
            .ContainSingle();


        result[0].CommonName
            .Should()
            .Be("Douglas Fir");
    }



    [Fact]
    public async Task SearchSpeciesHandler_WithNoMatch_ReturnsEmptyList()
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
            new SearchSpeciesHandler(
                repository.Object);


        // Act
        var result =
            await handler.Handle(
                new SearchSpeciesQuery("Oak"));


        // Assert
        result.Should()
            .BeEmpty();
    }
}