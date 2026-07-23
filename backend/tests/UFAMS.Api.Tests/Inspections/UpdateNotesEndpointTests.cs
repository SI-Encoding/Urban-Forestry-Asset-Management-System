using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Features.Inspections.UpdateNotes;
using UFAMS.Domain.Enums;
using Xunit;

namespace UFAMS.Api.Tests.Inspections;

public class UpdateNotesEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public UpdateNotesEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task UpdateNotes_WithExistingInspection_UpdatesNotes()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var treesResponse =
            await client.GetAsync("/trees");


        var trees =
            await treesResponse.Content
                .ReadFromJsonAsync<List<TreeDto>>();


        trees.Should()
            .NotBeEmpty();


        var treeId =
            trees![0].Id;


        var createCommand =
            new CreateInspectionCommand(
                DateOnly.FromDateTime(DateTime.Today),
                TreeHealthStatus.Good,
                "Initial notes",
                "Initial recommendation",
                null);


        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/inspections",
                createCommand);


        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var inspection =
            await createResponse.Content
                .ReadFromJsonAsync<InspectionDto>();


        inspection.Should()
            .NotBeNull();


        var command =
            new UpdateInspectionNotesCommand(
                "Updated inspection notes");


        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/inspections/{inspection!.Id}/notes",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var result =
            await response.Content
                .ReadFromJsonAsync<UpdateInspectionNotesResponse>();


        result.Should()
            .NotBeNull();


        result!.Id
            .Should()
            .Be(inspection.Id);


        result.Notes
            .Should()
            .Be("Updated inspection notes");
    }


    [Fact]
    public async Task UpdateNotes_WithInvalidInspection_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var command =
            new UpdateInspectionNotesCommand(
                "Updated notes");


        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/inspections/{Guid.NewGuid()}/notes",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }


    private sealed record TreeDto(
        Guid Id,
        string AssetTag);


    private sealed record InspectionDto(
        Guid Id,
        Guid TreeId,
        DateOnly InspectionDate,
        string ObservedHealth,
        string Notes,
        string Recommendation,
        DateOnly? NextInspectionDate);
}