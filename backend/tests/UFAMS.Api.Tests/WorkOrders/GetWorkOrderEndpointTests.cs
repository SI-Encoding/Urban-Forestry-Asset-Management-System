using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using Xunit;

namespace UFAMS.Api.Tests.WorkOrders;

public class GetWorkOrderEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GetWorkOrderEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWorkOrder_WithExistingId_ReturnsWorkOrder()
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
                "Inspect hanging branches",
                DateOnly.FromDateTime(
                    DateTime.Today.AddDays(30)));

        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/work-orders",
                createCommand);

        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var created =
            await createResponse.Content
                .ReadFromJsonAsync<CreateWorkOrderDto>();

        created.Should().NotBeNull();

        var workOrderId =
            created!.Id;


        // Act
        var response =
            await client.GetAsync(
                $"/work-orders/{workOrderId}");


        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var workOrder =
            await response.Content
                .ReadFromJsonAsync<GetWorkOrderDto>();


        workOrder.Should().NotBeNull();

        workOrder!.Id
            .Should()
            .Be(workOrderId);

        workOrder.TreeId
            .Should()
            .Be(treeId);

        workOrder.Description
            .Should()
            .Be("Inspect hanging branches");

        workOrder.Status
            .Should()
            .Be("Open");
    }


    [Fact]
    public async Task GetWorkOrder_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.GetAsync(
                $"/work-orders/{Guid.NewGuid()}");


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

    private sealed record GetWorkOrderDto(
        Guid Id,
        Guid TreeId,
        string Description,
        string Status,
        DateOnly? DueDate,
        Guid? AssignedEmployeeId);
}