using UFAMS.Domain.Enums;

public sealed record GetInspectionsResponse(
    Guid Id,
    Guid TreeId,
    string AssetTag,
    string SpeciesName,
    string ParkName,
    DateOnly InspectionDate,
    TreeHealthStatus ObservedHealth,
    string Recommendation,
    DateOnly? NextInspectionDate
);