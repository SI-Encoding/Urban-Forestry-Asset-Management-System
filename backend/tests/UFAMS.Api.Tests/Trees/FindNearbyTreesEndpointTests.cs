using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Application.Features.Trees.FindNearbyTrees;
using UFAMS.Api.Tests.Common;

namespace UFAMS.Api.Tests.Trees;

public class FindNearbyTreesEndpointTests
    : ApiTestBase
{
    public FindNearbyTreesEndpointTests(
        CustomWebApplicationFactory factory)
        : base(factory)
    {
        factory.SeedDatabase();
    }


    [Fact]
    public async Task FindNearbyTrees_ReturnsTreesWithinRadius()
    {
        // Stanley Park tree coordinates
        var response =
            await Client.GetAsync(
                "/trees/nearby?latitude=49.3043&longitude=-123.1443&radiusMeters=100");

        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.OK);


        var trees =
            await response.Content.ReadFromJsonAsync<
                List<FindNearbyTreesResponse>>();


        trees.Should()
            .NotBeNull();

        trees.Should()
            .NotBeEmpty();


        trees!
            .First()
            .DistanceMeters
            .Should()
            .BeLessThanOrEqualTo(100);
    }


    [Fact]
    public async Task FindNearbyTrees_WithInvalidRadius_ReturnsBadRequest()
    {
        var response =
            await Client.GetAsync(
                "/trees/nearby?latitude=49.3043&longitude=-123.1443&radiusMeters=0");


        response.StatusCode
            .Should()
            .Be(System.Net.HttpStatusCode.BadRequest);
    }
}