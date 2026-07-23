using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using Xunit;

namespace UFAMS.Api.Tests.WorkOrders;

public class GetTreeWorkOrdersEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GetTreeWorkOrdersEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTreeWorkOrders_WithExistingTree_ReturnsWorkOrders()
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

        trees.Should().NotBeNull();
        trees!.Should().NotBeEmpty();

        var treeId =
            trees![0].Id;

        var createCommand =
            new CreateWorkOrderCommand(
                "Prune lower branches",
                DateOnly.FromDateTime(
                    DateTime.Today.AddDays(30)));

        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/work-orders",
                createCommand);

        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        // Act
        var response =
            await client.GetAsync(
                $"/trees/{treeId}/work-orders");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var workOrders =
            await response.Content
                .ReadFromJsonAsync<List<WorkOrderDto>>();

        workOrders.Should().NotBeNull();

        workOrders!
            .Should()
            .ContainSingle();

        workOrders![0].TreeId
            .Should()
            .Be(treeId);

        workOrders[0].Description
            .Should()
            .Be("Prune lower branches");

        workOrders[0].Status
            .Should()
            .Be("Open");
    }


    [Fact]
    public async Task GetTreeWorkOrders_WithNoWorkOrders_ReturnsEmptyList()
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

        trees.Should().NotBeNull();
        trees!.Should().NotBeEmpty();

        // Choose another tree that has no work orders
        var treeId =
            trees![1].Id;

        // Act
        var response =
            await client.GetAsync(
                $"/trees/{treeId}/work-orders");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var workOrders =
            await response.Content
                .ReadFromJsonAsync<List<WorkOrderDto>>();

        workOrders.Should().NotBeNull();

        workOrders!
            .Should()
            .BeEmpty();
    }


    private sealed record TreeDto(
        Guid Id,
        string AssetTag,
        string Species,
        string Park,
        double Latitude,
        double Longitude,
        string HealthStatus);

    private sealed record WorkOrderDto(
        Guid Id,
        Guid TreeId,
        string Description,
        string Status,
        DateOnly? DueDate);
}