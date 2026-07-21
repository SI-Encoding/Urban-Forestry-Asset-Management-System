using UFAMS.Application.Features.Trees.RegisterTree;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Features.Trees.GetTree;
using UFAMS.Application.Features.Trees.UpdateMeasurements;
using UFAMS.Application.Features.Trees.RelocateTree;
using UFAMS.Application.Features.Trees.UpdateHealth;
using UFAMS.Domain.Enums;
namespace UFAMS.Api.Endpoints;

public static class TreeEndpoints
{
    public static IEndpointRouteBuilder MapTreeEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/trees",
            async (
                RegisterTreeCommand command,
                RegisterTreeHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    command,
                    cancellationToken);

                return Results.Created(
                    $"/trees/{response.Id}",
                    response);
            });

        app.MapGet(
            "/trees",
            async (
                Guid? parkId,
                Guid? speciesId,
                TreeHealthStatus? healthStatus,
                GetTreesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetTreesQuery(
                    parkId,
                    speciesId,
                    healthStatus);

                var response = await handler.Handle(
                    query,
                    cancellationToken);

                return Results.Ok(response);
            });

        app.MapGet(
            "/trees/{id:guid}",
            async (
                Guid id,
                GetTreeHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetTreeQuery(id),
                    cancellationToken);

                return Results.Ok(response);
            });

        app.MapPut(
            "/trees/{id:guid}/measurements",
            async (
                Guid id,
                UpdateTreeMeasurementsCommand command,
                UpdateTreeMeasurementsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return Results.Ok(response);
            });

        app.MapPut(
            "/trees/{id:guid}/location",
            async (
                Guid id,
                RelocateTreeCommand command,
                RelocateTreeHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return Results.Ok(response);
            });

        app.MapPut(
            "/trees/{id:guid}/health",
            async (
                Guid id,
                UpdateTreeHealthCommand command,
                UpdateTreeHealthHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return Results.Ok(response);
            });

        return app;
    }
}