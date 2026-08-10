using UFAMS.Application.Features.ArcGisSync;
using UFAMS.Application.Interfaces;

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

        app.MapPost(
            "/arcgis/sync/apply",
            async (
                SpatialDataSyncService syncService,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await syncService.ApplyAsync(
                        cancellationToken);

                return Results.Ok(result);
            })
        .WithName("ApplyArcGisSync")
        .WithSummary("Apply ArcGIS synchronization")
        .WithDescription(
            "Applies ArcGIS changes to UFAMS.");

        app.MapPost(
            "/arcgis/sync/apply/{assetTag}",
            async (
                string assetTag,
                SpatialDataSyncService syncService,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(assetTag))
                {
                    return Results.BadRequest(
                        "Asset tag is required.");
                }

                var result =
                    await syncService.ApplySingleAsync(
                        assetTag,
                        cancellationToken);

                return Results.Ok(result);
            });
        
        app.MapGet(
            "/arcgis/sync/audits",
            async (
                ISyncAuditRepository auditRepository,
                CancellationToken cancellationToken) =>
            {
                var audits =
                    await auditRepository.GetRecentAsync(
                        50,
                        cancellationToken);

                var result =
                    audits.Select(
                        audit => new
                        {
                            audit.Id,
                            audit.StartedAt,
                            audit.CompletedAt,
                            Status =
                                audit.Status.ToString(),
                            audit.CreatedCount,
                            audit.UpdatedCount,
                            audit.FailedCount,
                            audit.IgnoredCount,

                            Entries =
                                audit.Entries.Select(
                                    entry => new
                                    {
                                        entry.Id,
                                        entry.AssetTag,
                                        entry.Action,
                                        entry.Reason,
                                        entry.CreatedAt
                                    })
                        });

                return Results.Ok(result);
            })
        .WithName("GetSyncAudits")
        .WithSummary("Get recent ArcGIS synchronization audits")
        .WithDescription(
            "Returns recent ArcGIS synchronization audit history.");
            
        return app;
    }
}