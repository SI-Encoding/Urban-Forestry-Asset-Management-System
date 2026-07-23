namespace UFAMS.Application.Features.Trees.ExportTreesGeoJson;

public sealed record TreeGeoJsonProperties(
    Guid Id,
    string AssetTag,
    string Species,
    string Park,
    string HealthStatus);