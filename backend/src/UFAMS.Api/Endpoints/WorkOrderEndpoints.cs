using UFAMS.Application.Features.WorkOrders.CreateWorkOrder;
using UFAMS.Application.Features.WorkOrders.GetTreeWorkOrders;

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

        return app;
    }
}