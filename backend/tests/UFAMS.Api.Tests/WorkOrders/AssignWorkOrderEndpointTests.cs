using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.WorkOrders.AssignWorkOrder;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using Xunit;

namespace UFAMS.Api.Tests.WorkOrders;

public class AssignWorkOrderEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AssignWorkOrderEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AssignWorkOrder_WithExistingWorkOrder_ReturnsUpdatedWorkOrder()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();

        var trees =
            await client.GetFromJsonAsync<List<TreeDto>>("/trees");

        trees.Should().NotBeNull();
        trees!.Should().NotBeEmpty();

        var treeId =
            trees![0].Id;

        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/work-orders",
                new CreateWorkOrderCommand(
                    "Trim hazardous branches",
                    DateOnly.FromDateTime(
                        DateTime.Today.AddDays(30))));

        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<CreateWorkOrderDto>();

        created.Should().NotBeNull();

        var employeeId =
            _factory.GetEmployeeId("John Smith");

        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/work-orders/{created!.Id}/assign",
                new AssignWorkOrderCommand(employeeId));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<AssignWorkOrderDto>();

        result.Should().NotBeNull();

        result!.Id
            .Should()
            .Be(created.Id);

        result.AssignedEmployeeId
            .Should()
            .Be(employeeId);

        result.Status
            .Should()
            .Be("Assigned");
    }

    [Fact]
    public async Task AssignWorkOrder_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();

        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/work-orders/{Guid.NewGuid()}/assign",
                new AssignWorkOrderCommand(Guid.NewGuid()));

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

    private sealed record AssignWorkOrderDto(
        Guid Id,
        Guid TreeId,
        Guid? AssignedEmployeeId,
        string Status);
}