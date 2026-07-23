using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Features.Trees.RelocateTree;
using UFAMS.Application.Features.Parks.GetParks;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Api.Tests.Trees;

public class RelocateTreeEndpointTests
    : ApiTestBase
{
    private static readonly CustomWebApplicationFactory Factory =
        new();


    public RelocateTreeEndpointTests()
        : base(Factory)
    {
    }


    [Fact]
    public async Task RelocateTree_WithExistingTree_ReturnsUpdatedLocation()
    {
        // Arrange
        var treesResponse =
            await Client.GetAsync("/trees");


        var trees =
            await treesResponse.Content
                .ReadFromJsonAsync<
                    List<GetTreesResponse>>(
                        JsonOptions);


        trees.Should()
            .NotBeNull();


        trees!
            .Should()
            .NotBeEmpty();


        var tree =
            trees!
                .First();


        var parksResponse =
            await Client.GetAsync("/parks");


        var parks =
            await parksResponse.Content
                .ReadFromJsonAsync<
                    List<GetParksResponse>>(
                        JsonOptions);


        parks.Should()
            .NotBeNull();


        parks!
            .Should()
            .NotBeEmpty();


        var park =
            parks!
                .Last();


        var newLocation =
            new GeoCoordinate(
                49.2500,
                -123.1000);


        var command =
            new RelocateTreeCommand(
                park.Id,
                newLocation);


        // Act
        var response =
            await Client.PutAsJsonAsync(
                $"/trees/{tree.Id}/location",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var result =
            await response.Content
                .ReadFromJsonAsync<
                    RelocateTreeResponse>(
                        JsonOptions);


        result.Should()
            .NotBeNull();


        result!
            .ParkName
            .Should()
            .Be(park.Name);


        result.Location.Latitude
            .Should()
            .Be(49.2500);


        result.Location.Longitude
            .Should()
            .Be(-123.1000);
    }


    [Fact]
    public async Task RelocateTree_WithMissingTree_ReturnsNotFound()
    {
        // Arrange
        var command =
            new RelocateTreeCommand(
                Guid.NewGuid(),
                new GeoCoordinate(
                    49.2500,
                    -123.1000));


        // Act
        var response =
            await Client.PutAsJsonAsync(
                $"/trees/{Guid.NewGuid()}/location",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}