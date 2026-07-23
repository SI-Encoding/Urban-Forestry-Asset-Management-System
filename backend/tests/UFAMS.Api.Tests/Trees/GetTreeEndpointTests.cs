using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Trees.GetTree;
using UFAMS.Application.Features.Trees.GetTrees;

namespace UFAMS.Api.Tests.Trees;

public class GetTreeEndpointTests
    : ApiTestBase
{
    private static readonly CustomWebApplicationFactory Factory =
        new();


    public GetTreeEndpointTests()
        : base(Factory)
    {
    }


    [Fact]
    public async Task GetTree_WithExistingId_ReturnsTree()
    {
        // Arrange
        var treesResponse =
            await Client.GetAsync("/trees");


        treesResponse.EnsureSuccessStatusCode();


        var trees =
            await treesResponse.Content.ReadFromJsonAsync<
                List<GetTreesResponse>>(
                    JsonOptions);


        trees.Should()
            .NotBeNull();


        trees.Should()
            .NotBeEmpty();


        var treeId =
            trees!
                .First()
                .Id;


        treeId.Should()
            .NotBe(Guid.Empty);


        // Act
        var response =
            await Client.GetAsync(
                $"/trees/{treeId}");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var tree =
            await response.Content.ReadFromJsonAsync<
                GetTreeResponse>(
                    JsonOptions);


        tree.Should()
            .NotBeNull();


        tree!.Id
            .Should()
            .Be(treeId);
    }


    [Fact]
    public async Task GetTree_WithMissingId_ReturnsNotFound()
    {
        var response =
            await Client.GetAsync(
                $"/trees/{Guid.NewGuid()}");


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}