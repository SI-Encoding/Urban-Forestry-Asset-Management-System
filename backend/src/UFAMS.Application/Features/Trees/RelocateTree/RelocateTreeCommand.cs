using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Features.Trees.RelocateTree;

public sealed record RelocateTreeCommand(
    Guid ParkId,
    GeoCoordinate Location);