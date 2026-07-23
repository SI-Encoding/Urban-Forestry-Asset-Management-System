using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Trees.UpdateMeasurements;
using UFAMS.Application.Features.Trees.GetTrees;

namespace UFAMS.Api.Tests.Trees;

public class UpdateTreeMeasurementsEndpointTests
    : ApiTestBase
{
    private static readonly CustomWebApplicationFactory Factory =
        new();


    public UpdateTreeMeasurementsEndpointTests()
        : base(Factory)
    {
    }


    [Fact]
    public async Task UpdateMeasurements_WithExistingTree_ReturnsUpdatedMeasurements()
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
            trees![0].Id;


        var command =
            new UpdateTreeMeasurementsCommand(
                25,
                60);


        // Act
        var response =
            await Client.PutAsJsonAsync(
                $"/trees/{treeId}/measurements",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var result =
            await response.Content
                .ReadFromJsonAsync<
                    UpdateTreeMeasurementsResponse>(
                        JsonOptions);


        result.Should()
            .NotBeNull();


        result!.HeightInMeters
            .Should()
            .Be(25);


        result.DiameterInCentimeters
            .Should()
            .Be(60);
    }


    [Fact]
    public async Task UpdateMeasurements_WithMissingTree_ReturnsNotFound()
    {
        // Arrange
        var command =
            new UpdateTreeMeasurementsCommand(
                25,
                60);


        // Act
        var response =
            await Client.PutAsJsonAsync(
                $"/trees/{Guid.NewGuid()}/measurements",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}