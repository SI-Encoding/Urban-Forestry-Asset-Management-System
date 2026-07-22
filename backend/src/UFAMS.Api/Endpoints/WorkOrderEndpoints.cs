using UFAMS.Application.Features.WorkOrders.AssignWorkOrder;
using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using UFAMS.Application.Features.WorkOrders.GetTreeWorkOrders;
using UFAMS.Application.Features.WorkOrders.GetWorkOrder;

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
            });

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
            });

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
            });

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
            });

        return app;
    }
}