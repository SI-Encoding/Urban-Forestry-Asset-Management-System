using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Features.Trees.UpdateHealth;
using UFAMS.Domain.Enums;

namespace UFAMS.Api.Tests.Trees;

public class UpdateTreeHealthEndpointTests
    : ApiTestBase
{
    private static readonly CustomWebApplicationFactory Factory =
        new();


    public UpdateTreeHealthEndpointTests()
        : base(Factory)
    {
    }


    [Fact]
    public async Task UpdateHealth_WithExistingTree_ReturnsUpdatedHealth()
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


        var treeId =
            trees!
                .First()
                .Id;


        var command =
            new UpdateTreeHealthCommand(
                TreeHealthStatus.Poor);


        // Act
        var response =
            await Client.PutAsJsonAsync(
                $"/trees/{treeId}/health",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var result =
            await response.Content
                .ReadFromJsonAsync<
                    UpdateTreeHealthResponse>(
                        JsonOptions);


        result.Should()
            .NotBeNull();


        result!
            .HealthStatus
            .Should()
            .Be(TreeHealthStatus.Poor);
    }


    [Fact]
    public async Task UpdateHealth_WithMissingTree_ReturnsNotFound()
    {
        // Arrange
        var command =
            new UpdateTreeHealthCommand(
                TreeHealthStatus.Poor);


        // Act
        var response =
            await Client.PutAsJsonAsync(
                $"/trees/{Guid.NewGuid()}/health",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}