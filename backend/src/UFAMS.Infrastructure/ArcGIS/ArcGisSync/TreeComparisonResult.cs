namespace UFAMS.Application.Features.ArcGisSync;

public sealed record TreeComparisonResult(
    bool HasChanges,
    bool HealthChanged,
    bool SpeciesChanged,
    bool ParkChanged,
    bool LocationChanged);