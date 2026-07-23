namespace UFAMS.Application.Features.Species.SearchSpecies;

public sealed record SearchSpeciesResponse(
    Guid Id,
    string CommonName,
    string ScientificName,
    bool IsNative);