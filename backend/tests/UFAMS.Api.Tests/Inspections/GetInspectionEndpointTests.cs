using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;

namespace UFAMS.Api.Tests.Inspections;

public class GetInspectionEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GetInspectionEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task GetInspection_WithExistingId_ReturnsInspection()
    {
        // Arrange
        _factory.SeedDatabase();

        var client = _factory.CreateClient();


        var treesResponse =
            await client.GetAsync("/trees");


        var trees =
            await treesResponse.Content
                .ReadFromJsonAsync<List<TreeDto>>();


        trees.Should()
            .NotBeNull();

        trees!
            .Should()
            .NotBeEmpty();


        var treeId = trees![0].Id;


        var createRequest = new
        {
            InspectionDate = DateOnly.FromDateTime(DateTime.Today),
            ObservedHealth = "Good",
            Notes = "Routine inspection",
            Recommendation = "Continue monitoring",
            NextInspectionDate = DateOnly.FromDateTime(
                DateTime.Today.AddMonths(6))
        };


        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/inspections",
                createRequest);


        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var createdInspection =
            await createResponse.Content
                .ReadFromJsonAsync<InspectionDto>();


        createdInspection.Should()
            .NotBeNull();


        var inspectionId =
            createdInspection!.Id;



        // Act
        var response =
            await client.GetAsync(
                $"/inspections/{inspectionId}");



        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var inspection =
            await response.Content
                .ReadFromJsonAsync<InspectionDto>();


        inspection.Should()
            .NotBeNull();


        inspection!.Id
            .Should()
            .Be(inspectionId);


        inspection.TreeId
            .Should()
            .Be(treeId);
    }



    [Fact]
    public async Task GetInspection_WithMissingId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var missingId =
            Guid.NewGuid();


        // Act
        var response =
            await client.GetAsync(
                $"/inspections/{missingId}");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }



    private sealed record TreeDto(
        Guid Id,
        string AssetTag,
        string SpeciesName,
        string ParkName,
        object Location,
        string HealthStatus);



    private sealed record InspectionDto(
        Guid Id,
        Guid TreeId,
        DateOnly InspectionDate,
        string ObservedHealth,
        string Notes,
        string Recommendation,
        DateOnly? NextInspectionDate);
}