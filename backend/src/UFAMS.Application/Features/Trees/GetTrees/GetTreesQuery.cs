using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Trees.GetTrees;

public sealed record GetTreesQuery(
    Guid? ParkId,
    Guid? SpeciesId,
    TreeHealthStatus? HealthStatus);