using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Inspections.CreateInspection;

public sealed record CreateInspectionCommand(
    DateOnly InspectionDate,
    TreeHealthStatus ObservedHealth,
    string Notes,
    string Recommendation,
    DateOnly? NextInspectionDate);