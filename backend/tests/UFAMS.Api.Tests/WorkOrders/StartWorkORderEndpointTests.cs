using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.WorkOrders.AssignWorkOrder;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using Xunit;

namespace UFAMS.Api.Tests.WorkOrders;

public class StartWorkOrderEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public StartWorkOrderEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task StartWorkOrder_WithAssignedWorkOrder_ReturnsStartedWorkOrder()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();

        var trees =
            await client.GetFromJsonAsync<List<TreeDto>>(
                "/trees");

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

        var assignResponse =
            await client.PutAsJsonAsync(
                $"/work-orders/{created!.Id}/assign",
                new AssignWorkOrderCommand(
                    employeeId));

        assignResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        // Act
        var response =
            await client.PutAsync(
                $"/work-orders/{created.Id}/start",
                null);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<StartWorkOrderDto>();

        result.Should().NotBeNull();

        result!.Id
            .Should()
            .Be(created.Id);

        result.Status
            .Should()
            .Be("InProgress");
    }

    [Fact]
    public async Task StartWorkOrder_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();

        // Act
        var response =
            await client.PutAsync(
                $"/work-orders/{Guid.NewGuid()}/start",
                null);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    private sealed record TreeDto(
        Guid Id);

    private sealed record CreateWorkOrderDto(
        Guid Id,
        Guid TreeId,
        string Description,
        string Status,
        DateOnly? DueDate);

    private sealed record StartWorkOrderDto(
        Guid Id,
        Guid TreeId,
        string Status);
}