using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Trees.ExportTreesGeoJson;

public sealed record ExportTreesGeoJsonQuery(
    Guid? ParkId = null,
    Guid? SpeciesId = null,
    TreeHealthStatus? HealthStatus = null,
    double? MinLatitude = null,
    double? MaxLatitude = null,
    double? MinLongitude = null,
    double? MaxLongitude = null);