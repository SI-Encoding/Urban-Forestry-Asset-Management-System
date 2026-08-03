namespace UFAMS.Application.Features.ArcGisSync.Models;

public sealed record SpatialSyncResult(
    int Created,
    int Updated,
    int Deleted,
    int Unchanged,
    IReadOnlyList<SpatialSyncAction> Actions);