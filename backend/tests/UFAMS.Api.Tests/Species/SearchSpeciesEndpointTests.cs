using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using Xunit;

namespace UFAMS.Api.Tests.Species;

public class SearchSpeciesEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;


    public SearchSpeciesEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task SearchSpecies_WithMatchingQuery_ReturnsSpecies()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.GetAsync(
                "/species/search?query=fir");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var species =
            await response.Content
                .ReadFromJsonAsync<List<SpeciesDto>>();


        species.Should()
            .NotBeNull();


        species!
            .Should()
            .ContainSingle();


        species![0].CommonName
            .Should()
            .Be("Douglas Fir");


        species[0].ScientificName
            .Should()
            .Be("Pseudotsuga menziesii");


        species[0].IsNative
            .Should()
            .BeTrue();
    }



    [Fact]
    public async Task SearchSpecies_WithNoMatch_ReturnsEmptyList()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.GetAsync(
                "/species/search?query=Oak");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var species =
            await response.Content
                .ReadFromJsonAsync<List<SpeciesDto>>();


        species.Should()
            .NotBeNull();


        species!
            .Should()
            .BeEmpty();
    }



    [Fact]
    public async Task SearchSpecies_WithScientificName_ReturnsSpecies()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.GetAsync(
                "/species/search?query=Pseudotsuga");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var species =
            await response.Content
                .ReadFromJsonAsync<List<SpeciesDto>>();


        species.Should()
            .NotBeNull();


        species!
            .Should()
            .ContainSingle();


        species![0].ScientificName
            .Should()
            .Be("Pseudotsuga menziesii");
    }



    private sealed record SpeciesDto(
        Guid Id,
        string CommonName,
        string ScientificName,
        bool IsNative);
}