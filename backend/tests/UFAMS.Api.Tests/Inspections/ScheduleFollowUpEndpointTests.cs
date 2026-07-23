using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Features.Inspections.ScheduleFollowUp;
using UFAMS.Domain.Enums;
using Xunit;

namespace UFAMS.Api.Tests.Inspections;

public class ScheduleFollowUpEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ScheduleFollowUpEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task ScheduleFollowUp_WithExistingInspection_UpdatesDate()
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
                "Healthy tree",
                "Continue monitoring",
                null);


        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/inspections",
                createCommand);


        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var created =
            await createResponse.Content
                .ReadFromJsonAsync<InspectionDto>();


        created.Should()
            .NotBeNull();


        var followUpDate =
            DateOnly.FromDateTime(
                DateTime.Today.AddMonths(12));


        var command =
            new ScheduleFollowUpCommand(
                followUpDate);


        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/inspections/{created!.Id}/follow-up",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var result =
            await response.Content
                .ReadFromJsonAsync<ScheduleFollowUpResponse>();


        result.Should()
            .NotBeNull();


        result!.Id
            .Should()
            .Be(created.Id);


        result.NextInspectionDate
            .Should()
            .Be(followUpDate);
    }


    [Fact]
    public async Task ScheduleFollowUp_WithInvalidInspection_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var command =
            new ScheduleFollowUpCommand(
                DateOnly.FromDateTime(
                    DateTime.Today.AddMonths(6)));


        var invalidId =
            Guid.NewGuid();


        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/inspections/{invalidId}/follow-up",
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