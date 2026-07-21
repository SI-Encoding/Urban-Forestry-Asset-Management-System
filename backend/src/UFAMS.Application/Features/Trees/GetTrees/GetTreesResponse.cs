using UFAMS.Domain.ValueObjects;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Trees.GetTrees;

public sealed record GetTreesResponse(
    Guid Id,
    string AssetTag,
    string SpeciesName,
    string ParkName,
    GeoCoordinate Location,
    TreeHealthStatus HealthStatus);