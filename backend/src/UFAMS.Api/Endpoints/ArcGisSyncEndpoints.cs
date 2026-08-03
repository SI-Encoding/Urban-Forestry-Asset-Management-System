using UFAMS.Application.Features.ArcGisSync;

namespace UFAMS.Api.Endpoints;

public static class ArcGisSyncEndpoints
{
    public static IEndpointRouteBuilder MapArcGisSyncEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/arcgis/sync/preview",
            async (
                SpatialDataSyncService syncService,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await syncService.SynchronizeAsync(
                        cancellationToken);

                return Results.Ok(result);
            })
        .WithName("PreviewArcGisSync")
        .WithSummary("Preview ArcGIS synchronization")
        .WithDescription(
            "Returns a synchronization plan without modifying the database.");

        return app;
    }
}