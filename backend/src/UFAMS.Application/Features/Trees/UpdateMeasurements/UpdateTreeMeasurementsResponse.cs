using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Features.Trees.UpdateMeasurements;

public sealed record UpdateTreeMeasurementsResponse(
    Guid Id,
    string AssetTag,
    double HeightInMeters,
    double DiameterInCentimeters,
    TreeHealthStatus HealthStatus);