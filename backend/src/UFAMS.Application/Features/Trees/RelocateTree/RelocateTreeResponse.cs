using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Features.Trees.RelocateTree;

public sealed record RelocateTreeResponse(
    Guid Id,
    string AssetTag,
    string ParkName,
    GeoCoordinate Location);