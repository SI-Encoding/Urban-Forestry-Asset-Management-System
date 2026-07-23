using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using UFAMS.Domain.Enums;
using Xunit;

namespace UFAMS.Api.Tests.WorkOrders;

public class CreateWorkOrderEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CreateWorkOrderEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task CreateWorkOrder_WithExistingTree_ReturnsCreatedWorkOrder()
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
            .NotBeNull();


        trees!
            .Should()
            .NotBeEmpty();


        var treeId =
            trees![0].Id;


        var command =
            new CreateWorkOrderCommand(
                "Prune damaged branches",
                DateOnly.FromDateTime(
                    DateTime.Today.AddDays(30)));


        // Act
        var response =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/work-orders",
                command);


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var result =
            await response.Content
                .ReadFromJsonAsync<CreateWorkOrderDto>();


        result.Should()
            .NotBeNull();


        result!.TreeId
            .Should()
            .Be(treeId);


        result.Description
            .Should()
            .Be("Prune damaged branches");


        result.Status
            .Should()
            .Be("Open");


        result.DueDate
            .Should()
            .NotBeNull();
    }


    [Fact]
    public async Task CreateWorkOrder_WithInvalidTreeId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var command =
            new CreateWorkOrderCommand(
                "Remove dead tree",
                null);


        var invalidTreeId =
            Guid.NewGuid();


        // Act
        var response =
            await client.PostAsJsonAsync(
                $"/trees/{invalidTreeId}/work-orders",
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

    private sealed record CreateWorkOrderDto(
        Guid Id,
        Guid TreeId,
        string Description,
        string Status,
        DateOnly? DueDate);
}