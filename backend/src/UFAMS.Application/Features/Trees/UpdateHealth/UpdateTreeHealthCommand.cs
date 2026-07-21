using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Trees.UpdateHealth;

public sealed record UpdateTreeHealthCommand(
    TreeHealthStatus HealthStatus);