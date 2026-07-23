using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Parks.GetParks;
using Xunit;

namespace UFAMS.Api.Tests.Parks;

public class GetParksEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GetParksEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task GetParks_ReturnsParks()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.GetAsync("/parks");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var parks =
            await response.Content
                .ReadFromJsonAsync<List<GetParksResponse>>();


        parks.Should()
            .NotBeNull();


        parks!
            .Should()
            .NotBeEmpty();


        parks![0].Name
            .Should()
            .NotBeNullOrWhiteSpace();


        parks[0].AreaInHectares
            .Should()
            .BeGreaterThan(0);
    }


    [Fact]
    public async Task GetParks_ReturnsExpectedSeededPark()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.GetAsync("/parks");


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var parks =
            await response.Content
                .ReadFromJsonAsync<List<GetParksResponse>>();


        // Assert
        parks.Should()
            .NotBeNull();


        parks!
            .Should()
            .Contain(p =>
                p.Name == "Stanley Park");
    }
}