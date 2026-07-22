namespace UFAMS.Application.Features.Trees.FindNearbyTrees;

public sealed record FindNearbyTreesResponse(
    Guid Id,
    string AssetTag,
    string Species,
    string Park,
    double Latitude,
    double Longitude,
    double DistanceMeters);