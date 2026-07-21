using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Inspections.GetTreeInspections;

public sealed record GetTreeInspectionsResponse(
    Guid Id,
    Guid TreeId,
    DateOnly InspectionDate,
    TreeHealthStatus ObservedHealth,
    string Notes,
    string Recommendation,
    DateOnly? NextInspectionDate);