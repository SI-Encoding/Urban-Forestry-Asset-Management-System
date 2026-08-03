namespace UFAMS.Infrastructure.ArcGIS.Models;

public sealed record ArcGisFeature(
    string Id,
    string AssetTag,
    string Species,
    string Park,
    string HealthStatus,
    double Latitude,
    double Longitude);