namespace UFAMS.Application.Features.Trees.SearchTrees;

public sealed record SearchTreesResponse(
    Guid Id,
    string AssetTag,
    string Species,
    string Park,
    double Latitude,
    double Longitude,
    string HealthStatus);