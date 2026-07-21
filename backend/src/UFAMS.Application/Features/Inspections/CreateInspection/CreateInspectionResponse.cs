using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Features.Inspections.CreateInspection;

public sealed record CreateInspectionResponse(
    Guid Id,
    Guid TreeId,
    DateOnly InspectionDate,
    TreeHealthStatus ObservedHealth,
    string Notes,
    string Recommendation,
    DateOnly? NextInspectionDate);