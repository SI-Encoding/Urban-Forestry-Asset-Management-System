namespace UFAMS.Application.Features.ArcGisSync.Models;

public sealed record SpatialSyncAction(
    string Action,
    string AssetTag,
    string Reason);