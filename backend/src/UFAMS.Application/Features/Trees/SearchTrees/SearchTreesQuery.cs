using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Trees.SearchTrees;

public sealed record SearchTreesQuery(
    Guid? ParkId,
    Guid? SpeciesId,
    TreeHealthStatus? HealthStatus,
    double? MinLatitude,
    double? MaxLatitude,
    double? MinLongitude,
    double? MaxLongitude);