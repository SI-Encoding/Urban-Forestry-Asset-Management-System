using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UFAMS.Api.Tests.Common;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using Xunit;

namespace UFAMS.Api.Tests.WorkOrders;

public class CancelWorkOrderEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CancelWorkOrderEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task CancelWorkOrder_WithExistingWorkOrder_ReturnsCancelledWorkOrder()
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


        var createResponse =
            await client.PostAsJsonAsync(
                $"/trees/{treeId}/work-orders",
                new CreateWorkOrderCommand(
                    "Remove fallen branch",
                    DateOnly.FromDateTime(
                        DateTime.Today.AddDays(10))));


        createResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);


        var created =
            await createResponse.Content
                .ReadFromJsonAsync<CreateWorkOrderDto>();


        created.Should()
            .NotBeNull();



        // Act
        var response =
            await client.PutAsync(
                $"/work-orders/{created!.Id}/cancel",
                null);



        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);



        var result =
            await response.Content
                .ReadFromJsonAsync<CancelWorkOrderDto>();


        result.Should()
            .NotBeNull();


        result!.Id
            .Should()
            .Be(created.Id);


        result.Status
            .Should()
            .Be("Cancelled");
    }



    [Fact]
    public async Task CancelWorkOrder_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        _factory.SeedDatabase();

        var client =
            _factory.CreateClient();



        // Act
        var response =
            await client.PutAsync(
                $"/work-orders/{Guid.NewGuid()}/cancel",
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


    private sealed record CancelWorkOrderDto(
        Guid Id,
        Guid TreeId,
        string Status);
}