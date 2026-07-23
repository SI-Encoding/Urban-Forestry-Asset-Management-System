using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Trees.SearchTrees;

namespace UFAMS.Api.Tests.Trees;

public class SearchTreesEndpointTests
    : ApiTestBase
{
    private static readonly CustomWebApplicationFactory Factory =
        new();


    public SearchTreesEndpointTests()
        : base(Factory)
    {
    }


    [Fact]
    public async Task SearchTrees_ByHealthStatus_ReturnsMatchingTrees()
    {
        // Act
        var response =
            await Client.GetAsync(
                "/trees/search?healthStatus=Good");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var trees =
            await response.Content
                .ReadFromJsonAsync<
                    List<SearchTreesResponse>>(
                        JsonOptions);


        trees.Should()
            .NotBeNull();


        trees!
            .Should()
            .NotBeEmpty();


        trees!
            .Should()
            .AllSatisfy(tree =>
            {
                tree.HealthStatus
                    .Should()
                    .Be("Good");
            });
    }


    [Fact]
    public async Task SearchTrees_ByLatitudeRange_ReturnsMatchingTrees()
    {
        // Act
        var response =
            await Client.GetAsync(
                "/trees/search?minLatitude=49.3&maxLatitude=49.31");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var trees =
            await response.Content
                .ReadFromJsonAsync<
                    List<SearchTreesResponse>>(
                        JsonOptions);


        trees.Should()
            .NotBeNull();


        trees!
            .Should()
            .ContainSingle();


        trees![0]
            .Park
            .Should()
            .Be("Stanley Park");
    }


    [Fact]
    public async Task SearchTrees_WithNoMatches_ReturnsEmptyList()
    {
        // Act
        var response =
            await Client.GetAsync(
                "/trees/search?minLatitude=0&maxLatitude=1");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var trees =
            await response.Content
                .ReadFromJsonAsync<
                    List<SearchTreesResponse>>(
                        JsonOptions);


        trees.Should()
            .NotBeNull();


        trees!
            .Should()
            .BeEmpty();
    }
}