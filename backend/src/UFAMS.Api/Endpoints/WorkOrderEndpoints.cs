using UFAMS.Application.Features.WorkOrders.AssignWorkOrder;
using UFAMS.Application.Features.WorkOrders.CancelWorkOrder;
using UFAMS.Application.Features.WorkOrders.CompleteWorkOrder;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using UFAMS.Application.Features.WorkOrders.GetTreeWorkOrders;
using UFAMS.Application.Features.WorkOrders.GetWorkOrder;
using UFAMS.Application.Features.WorkOrders.GetWorkOrders;
using UFAMS.Application.Features.WorkOrders.StartWorkOrder;

namespace UFAMS.Api.Endpoints;

public static class WorkOrderEndpoints
{
    public static IEndpointRouteBuilder MapWorkOrderEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/trees/{treeId:guid}/work-orders",
            async (
                Guid treeId,
                CreateWorkOrderCommand command,
                CreateWorkOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    treeId,
                    command,
                    cancellationToken);

                return Results.Created(
                    $"/work-orders/{response.Id}",
                    response);
            })
        .WithName("CreateWorkOrder")
        .WithSummary("Creates a new work order for a tree")
        .WithDescription(
            "Creates a new work order for a specific tree identified by its unique ID.");

        app.MapGet(
            "/trees/{treeId:guid}/work-orders",
            async (
                Guid treeId,
                GetTreeWorkOrdersHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetTreeWorkOrdersQuery(treeId),
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("GetTreeWorkOrders")
        .WithSummary("Returns work orders for a specific tree")
        .WithDescription(
            "Retrieves all work orders associated with a specific tree identified by its unique ID.");

        app.MapPut(
            "/work-orders/{id:guid}/assign",
            async (
                Guid id,
                AssignWorkOrderCommand command,
                AssignWorkOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("AssignWorkOrder")
        .WithSummary("Assigns a work order to a user")
        .WithDescription(
            "Assigns a specific work order to a user identified by their unique ID.");

        app.MapGet(
            "/work-orders",
            async (
                GetWorkOrdersHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        new GetWorkOrdersQuery(),
                        cancellationToken);

                return Results.Ok(response);
            })
        .WithName("GetWorkOrders")
        .WithSummary("Returns all work orders")
        .WithDescription(
            "Retrieves all work orders with related tree, species, park, and employee information.");

        app.MapGet(
            "/work-orders/{id:guid}",
            async (
                Guid id,
                GetWorkOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetWorkOrderQuery(id),
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("GetWorkOrder")
        .WithSummary("Returns a specific work order")
        .WithDescription(
            "Retrieves details for a specific work order identified by its unique ID.");

        app.MapPut(
            "/work-orders/{id:guid}/start",
            async (
                Guid id,
                StartWorkOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        id,
                        cancellationToken);

                return Results.Ok(response);
            })
        .WithName("StartWorkOrder")
        .WithSummary("Starts a work order")
        .WithDescription(
            "Starts a specific work order identified by its unique ID.");

        app.MapPut(
            "/work-orders/{id:guid}/complete",
            async (
                Guid id,
                CompleteWorkOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        id,
                        cancellationToken);

                return Results.Ok(response);
            })
        .WithName("CompleteWorkOrder")
        .WithSummary("Completes a work order")
        .WithDescription(
            "Completes a specific work order identified by its unique ID.");

        app.MapPut(
            "/work-orders/{id:guid}/cancel",
            async (
                Guid id,
                CancelWorkOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        id,
                        cancellationToken);

                return Results.Ok(response);
            })
        .WithName("CancelWorkOrder")
        .WithSummary("Cancels a work order")
        .WithDescription(
            "Cancels a specific work order identified by its unique ID.");

        return app;
    }
}