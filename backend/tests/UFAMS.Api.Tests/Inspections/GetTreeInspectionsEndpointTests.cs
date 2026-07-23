using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Features.Inspections.GetTreeInspections;
using UFAMS.Domain.Enums;
using Xunit;

namespace UFAMS.Api.Tests.Inspections;

public class GetTreeInspectionsEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GetTreeInspectionsEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task GetTreeInspections_WithExistingTree_ReturnsInspections()
    {
        // Arrange
        _factory.SeedDatabase();

        var client = _factory.CreateClient();


        // Get an existing tree
        var treesResponse =
            await client.GetAsync("/trees");


        var trees =
            await treesResponse.Content
                .ReadFromJsonAsync<List<TreeInspectionDto>>();


        trees.Should()
            .NotBeEmpty();


        var treeId =
            trees![0].Id;


        // Create an inspection
        var createInspectionCommand =
            new CreateInspectionCommand(
                DateOnly.FromDateTime(DateTime.Today),
                TreeHealthStatus.Good,
                "Healthy tree",
                "Continue monitoring",
                DateOnly.FromDateTime(DateTime.Today.AddMonths(6)));


        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/inspections",
                createInspectionCommand);


        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        // Act
        var response =
            await client.GetAsync(
                $"/trees/{treeId}/inspections");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var inspections =
    await response.Content.ReadFromJsonAsync<
        List<TreeInspectionDto>>();

        inspections.Should()
            .NotBeNull();


        inspections!
            .Should()
            .ContainSingle();


        inspections![0].TreeId
            .Should()
            .Be(treeId);


        inspections[0].Notes
            .Should()
            .Be("Healthy tree");
    }


    [Fact]
    public async Task GetTreeInspections_WithNoInspections_ReturnsEmptyList()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var treesResponse =
            await client.GetAsync("/trees");


        var trees =
            await treesResponse.Content
                .ReadFromJsonAsync<List<TreeInspectionDto>>();


        trees.Should()
            .NotBeEmpty();


        // Pick a different tree that has no inspection
        var treeId =
            trees!
                .Last()
                .Id;


        // Act
        var response =
            await client.GetAsync(
                $"/trees/{treeId}/inspections");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var inspections =
            await response.Content
                .ReadFromJsonAsync<List<TreeInspectionDto>>();


        inspections.Should()
            .NotBeNull();


        inspections!
            .Should()
            .BeEmpty();
    }


    private sealed record TreeInspectionDto(
    Guid Id,
    Guid TreeId,
    DateOnly InspectionDate,
    string ObservedHealth,
    string Notes,
    string Recommendation,
    DateOnly? NextInspectionDate);
}