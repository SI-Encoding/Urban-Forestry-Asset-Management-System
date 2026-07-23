using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.WorkOrders.AssignWorkOrder;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using Xunit;

namespace UFAMS.Api.Tests.WorkOrders;

public class CompleteWorkOrderEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CompleteWorkOrderEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task CompleteWorkOrder_WithInProgressWorkOrder_ReturnsCompletedWorkOrder()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        var trees =
            await client.GetFromJsonAsync<List<TreeDto>>(
                "/trees");


        trees.Should()
            .NotBeNull();


        trees!
            .Should()
            .NotBeEmpty();


        var treeId =
            trees![0].Id;


        // Create work order
        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/work-orders",
                new CreateWorkOrderCommand(
                    "Remove damaged branch",
                    DateOnly.FromDateTime(
                        DateTime.Today.AddDays(14))));


        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var created =
            await createResponse.Content
                .ReadFromJsonAsync<CreateWorkOrderDto>();


        created.Should()
            .NotBeNull();


        // Assign employee
        var employeeId =
            _factory.GetEmployeeId(
                "John Smith");


        var assignResponse =
            await client.PutAsJsonAsync(
                $"/work-orders/{created!.Id}/assign",
                new AssignWorkOrderCommand(
                    employeeId));


        assignResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        // Start work order
        var startResponse =
            await client.PutAsync(
                $"/work-orders/{created.Id}/start",
                null);


        startResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);



        // Act
        var response =
            await client.PutAsync(
                $"/work-orders/{created.Id}/complete",
                null);



        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var result =
            await response.Content
                .ReadFromJsonAsync<CompleteWorkOrderDto>();


        result.Should()
            .NotBeNull();


        result!.Id
            .Should()
            .Be(created.Id);


        result.Status
            .Should()
            .Be("Completed");
    }



    [Fact]
    public async Task CompleteWorkOrder_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();


        // Act
        var response =
            await client.PutAsync(
                $"/work-orders/{Guid.NewGuid()}/complete",
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


    private sealed record CompleteWorkOrderDto(
        Guid Id,
        Guid TreeId,
        string Status);
}