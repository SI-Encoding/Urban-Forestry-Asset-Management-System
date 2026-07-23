using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Features.Inspections.UpdateRecommendation;
using UFAMS.Domain.Enums;
using Xunit;

namespace UFAMS.Api.Tests.Inspections;

public class UpdateRecommendationEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public UpdateRecommendationEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task UpdateRecommendation_WithExistingInspection_ReturnsUpdatedInspection()
    {
        // Arrange
        _factory.SeedDatabase();

        var client = _factory.CreateClient();


        // Get existing tree
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


        var treeId =
            trees![0].Id;


        // Create inspection
        var createCommand =
            new CreateInspectionCommand(
                DateOnly.FromDateTime(DateTime.Today),
                TreeHealthStatus.Good,
                "Healthy tree",
                "Continue monitoring",
                DateOnly.FromDateTime(DateTime.Today.AddMonths(6)));


        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/inspections",
                createCommand);


        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var createdInspection =
            await createResponse.Content
                .ReadFromJsonAsync<InspectionDto>();


        createdInspection.Should()
            .NotBeNull();


        // New recommendation
        var updateCommand =
            new UpdateInspectionRecommendationCommand(
                "Schedule pruning within three months");


        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/inspections/{createdInspection!.Id}/recommendation",
                updateCommand);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var updatedInspection =
            await response.Content
                .ReadFromJsonAsync<UpdateRecommendationResponseDto>();


        updatedInspection.Should()
            .NotBeNull();


        updatedInspection!.Id
            .Should()
            .Be(createdInspection.Id);


        updatedInspection.TreeId
            .Should()
            .Be(treeId);


        updatedInspection.Recommendation
            .Should()
            .Be("Schedule pruning within three months");
    }


    [Fact]
    public async Task UpdateRecommendation_WithInvalidInspectionId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var command =
            new UpdateInspectionRecommendationCommand(
                "Remove damaged branches");


        var invalidInspectionId =
            Guid.NewGuid();


        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/inspections/{invalidInspectionId}/recommendation",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }


    private sealed record TreeDto(
        Guid Id,
        string AssetTag,
        string Species,
        string Park,
        double Latitude,
        double Longitude,
        string HealthStatus);


    private sealed record InspectionDto(
        Guid Id,
        Guid TreeId,
        DateOnly InspectionDate,
        string ObservedHealth,
        string Notes,
        string Recommendation,
        DateOnly? NextInspectionDate);


    private sealed record UpdateRecommendationResponseDto(
        Guid Id,
        Guid TreeId,
        string Recommendation);
}