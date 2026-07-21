namespace UFAMS.Application.Features.Inspections.UpdateRecommendation;

public sealed record UpdateInspectionRecommendationResponse(
    Guid Id,
    Guid TreeId,
    string Recommendation);