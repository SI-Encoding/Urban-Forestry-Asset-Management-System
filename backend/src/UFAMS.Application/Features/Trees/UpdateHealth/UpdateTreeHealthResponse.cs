using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Trees.UpdateHealth;

public sealed record UpdateTreeHealthResponse(
    Guid Id,
    string AssetTag,
    TreeHealthStatus HealthStatus);