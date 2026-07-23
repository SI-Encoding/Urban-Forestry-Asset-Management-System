using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using Xunit;

namespace UFAMS.Api.Tests.Parks;

public class GetParkInventoryEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;


    public GetParkInventoryEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task GetParkInventory_WithExistingPark_ReturnsInventory()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var parksResponse =
            await client.GetAsync("/parks");


        var parks =
            await parksResponse.Content
                .ReadFromJsonAsync<List<ParkDto>>();


        parks.Should()
            .NotBeEmpty();


        var parkId =
            parks![0].Id;


        // Act
        var response =
            await client.GetAsync(
                $"/parks/{parkId}/inventory");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var inventory =
            await response.Content
                .ReadFromJsonAsync<InventoryDto>();


        inventory.Should()
            .NotBeNull();


        inventory!.ParkName
            .Should()
            .NotBeNullOrWhiteSpace();


        inventory.TotalTrees
            .Should()
            .BeGreaterThan(0);


        inventory.Species
            .Should()
            .NotBeEmpty();
    }



    [Fact]
    public async Task GetParkInventory_WithInvalidPark_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.GetAsync(
                $"/parks/{Guid.NewGuid()}/inventory");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }



    private sealed record ParkDto(
        Guid Id,
        string Name,
        double AreaInHectares);


    private sealed record InventoryDto(
        Guid ParkId,
        string ParkName,
        int TotalTrees,
        int HealthyTrees,
        int TreesNeedingAttention,
        List<SpeciesDto> Species);


    private sealed record SpeciesDto(
        string CommonName,
        string ScientificName,
        int Count);
}