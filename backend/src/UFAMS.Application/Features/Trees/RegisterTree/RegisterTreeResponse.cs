using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Features.Trees.RegisterTree;

public record RegisterTreeResponse(
    Guid Id,
    string AssetTag,
    string SpeciesName,
    string ParkName,
    GeoCoordinate Location,
    TreeHealthStatus HealthStatus);