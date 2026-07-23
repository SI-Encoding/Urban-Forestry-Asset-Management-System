using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Trees.RegisterTree;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Domain.ValueObjects;
using UFAMS.Domain.Entities;
namespace UFAMS.Api.Tests.Trees;

public class RegisterTreeEndpointTests
    : ApiTestBase
{
    private static readonly CustomWebApplicationFactory Factory =
        new();


    public RegisterTreeEndpointTests()
        : base(Factory)
    {
    }


    [Fact]
    public async Task RegisterTree_WithValidRequest_ReturnsCreated()
    {
        // Arrange

        var existingTreesResponse =
            await Client.GetAsync("/trees");


        var existingTrees =
            await existingTreesResponse.Content
                .ReadFromJsonAsync<
                    List<GetTreesResponse>>(
                        JsonOptions);


        existingTrees.Should()
            .NotBeNull();


        var speciesId =
            Factory.GetSpeciesId(
                "Douglas Fir");


        var parkId =
            Factory.GetParkId(
                "Stanley Park");


        var command =
            new RegisterTreeCommand(
                "TREE-003",
                speciesId,
                parkId,
                new GeoCoordinate(
                    49.2500,
                    -123.1200),
                new DateOnly(
                    2021,
                    1,
                    1),
                15,
                35);


        // Act

        var response =
            await Client.PostAsJsonAsync(
                "/trees",
                command);


        // Assert

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var result =
            await response.Content
                .ReadFromJsonAsync<
                    RegisterTreeResponse>(
                        JsonOptions);


        result.Should()
            .NotBeNull();


        result!.AssetTag
            .Should()
            .Be("TREE-003");


        result.HealthStatus
            .ToString()
            .Should()
            .Be("Good");
    }


    [Fact]
    public async Task RegisterTree_WithDuplicateAssetTag_ReturnsConflict()
    {
        var speciesId =
            Factory.GetSpeciesId(
                "Douglas Fir");


        var parkId =
            Factory.GetParkId(
                "Stanley Park");


        var command =
            new RegisterTreeCommand(
                "TREE-001",
                speciesId,
                parkId,
                new GeoCoordinate(
                    49.2500,
                    -123.1200),
                new DateOnly(
                    2021,
                    1,
                    1),
                15,
                35);


        var response =
            await Client.PostAsJsonAsync(
                "/trees",
                command);


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Conflict);
    }


    [Fact]
    public async Task RegisterTree_WithInvalidSpecies_ReturnsNotFound()
    {
        var parkId =
            Factory.GetParkId(
                "Stanley Park");


        var command =
            new RegisterTreeCommand(
                "TREE-004",
                Guid.NewGuid(),
                parkId,
                new GeoCoordinate(
                    49.2500,
                    -123.1200),
                new DateOnly(
                    2021,
                    1,
                    1),
                15,
                35);


        var response =
            await Client.PostAsJsonAsync(
                "/trees",
                command);


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task RegisterTree_WithInvalidPark_ReturnsNotFound()
    {
        var speciesId =
            Factory.GetSpeciesId(
                "Douglas Fir");


        var command =
            new RegisterTreeCommand(
                "TREE-005",
                speciesId,
                Guid.NewGuid(),
                new GeoCoordinate(
                    49.2500,
                    -123.1200),
                new DateOnly(
                    2021,
                    1,
                    1),
                15,
                35);


        var response =
            await Client.PostAsJsonAsync(
                "/trees",
                command);


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}