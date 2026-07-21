using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Features.Inspections.GetTreeInspections;

namespace UFAMS.Api.Endpoints;

public static class InspectionEndpoints
{
    public static IEndpointRouteBuilder MapInspectionEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/trees/{treeId:guid}/inspections",
            async (
                Guid treeId,
                CreateInspectionCommand command,
                CreateInspectionHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    treeId,
                    command,
                    cancellationToken);

                return Results.Created(
                    $"/inspections/{response.Id}",
                    response);
            });


        app.MapGet(
            "/trees/{treeId:guid}/inspections",
            async (
                Guid treeId,
                GetTreeInspectionsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetTreeInspectionsQuery(treeId),
                    cancellationToken);

                return Results.Ok(response);
            });


        return app;
    }
}