namespace UFAMS.Application.Features.ArcGisSync.Models;

public sealed record SpatialSyncAction(
    string Action,
    string AssetTag,
    string Reason,

    string? UfamsSpecies = null,
    string? ArcGisSpecies = null,

    string? UfamsPark = null,
    string? ArcGisPark = null,

    string? UfamsHealthStatus = null,
    string? ArcGisHealthStatus = null,

    double? UfamsLatitude = null,
    double? ArcGisLatitude = null,

    double? UfamsLongitude = null,
    double? ArcGisLongitude = null
);