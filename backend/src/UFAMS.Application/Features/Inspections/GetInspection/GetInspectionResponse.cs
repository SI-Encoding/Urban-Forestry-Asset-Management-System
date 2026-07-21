using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Inspections.GetInspection;

public sealed record GetInspectionResponse(
    Guid Id,
    Guid TreeId,
    DateOnly InspectionDate,
    TreeHealthStatus ObservedHealth,
    string Notes,
    string Recommendation,
    DateOnly? NextInspectionDate);