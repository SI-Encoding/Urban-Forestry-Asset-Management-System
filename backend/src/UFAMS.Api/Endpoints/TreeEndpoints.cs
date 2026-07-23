using UFAMS.Application.Features.Trees.RegisterTree;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Features.Trees.GetTree;
using UFAMS.Application.Features.Trees.UpdateMeasurements;
using UFAMS.Application.Features.Trees.RelocateTree;
using UFAMS.Application.Features.Trees.UpdateHealth;
using UFAMS.Application.Features.Trees.SearchTrees;
using UFAMS.Application.Features.Trees.ExportTreesGeoJson;
using UFAMS.Application.Features.Trees.FindNearbyTrees;
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
            })
        .WithName("RegisterTree")
        .WithSummary("Registers a new tree")
        .WithDescription(
            "Creates a new tree entry in the Urban Forest Asset Management System.");

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
            })
        .WithName("GetTrees")
        .WithSummary("Returns a list of trees")
        .WithDescription(
            "Retrieves a list of trees based on specified criteria.");

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
            })
        .WithName("GetTree")
        .WithSummary("Returns a specific tree")
        .WithDescription(
            "Retrieves details for a specific tree identified by its unique ID.");

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
            })
        .WithName("UpdateTreeMeasurements")
        .WithSummary("Updates tree measurements")
        .WithDescription(
            "Updates the measurements for a specific tree identified by its unique ID.");

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
            })
        .WithName("RelocateTree")
        .WithSummary("Relocates a tree")
        .WithDescription(
            "Updates the location for a specific tree identified by its unique ID.");

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
            })
        .WithName("UpdateTreeHealth")
        .WithSummary("Updates tree health status")
        .WithDescription(
            "Updates the health status for a specific tree identified by its unique ID.");

        app.MapGet(
            "/trees/search",
            async (
                Guid? parkId,
                Guid? speciesId,
                TreeHealthStatus? healthStatus,
                double? minLatitude,
                double? maxLatitude,
                double? minLongitude,
                double? maxLongitude,
                SearchTreesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new SearchTreesQuery(
                        parkId,
                        speciesId,
                        healthStatus,
                        minLatitude,
                        maxLatitude,
                        minLongitude,
                        maxLongitude),
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("SearchTrees")
        .WithSummary("Searches for trees")
        .WithDescription(
            "Searches for trees based on specified criteria.");

        app.MapGet(
            "/trees/geojson",
            async (
                Guid? parkId,
                Guid? speciesId,
                TreeHealthStatus? healthStatus,
                double? minLatitude,
                double? maxLatitude,
                double? minLongitude,
                double? maxLongitude,
                ExportTreesGeoJsonHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new ExportTreesGeoJsonQuery(
                        parkId,
                        speciesId,
                        healthStatus,
                        minLatitude,
                        maxLatitude,
                        minLongitude,
                        maxLongitude),
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("ExportTreesGeoJson")
        .WithSummary("Exports trees as GeoJSON")
        .WithDescription(
            "Exports tree data as GeoJSON for mapping and visualization purposes.");

        app.MapGet(
            "/trees/nearby",
            async (
                double latitude,
                double longitude,
                double radiusMeters,
                FindNearbyTreesHandler handler,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var response = await handler.Handle(
                        new FindNearbyTreesQuery(
                            latitude,
                            longitude,
                            radiusMeters),
                        cancellationToken);

                    return Results.Ok(response);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        message = ex.Message
                    });
                }
            })
        .WithName("FindNearbyTrees")
        .WithSummary("Finds nearby trees")
        .WithDescription(
            "Finds trees within a specified radius of a given location.");

        return app;
    }
}