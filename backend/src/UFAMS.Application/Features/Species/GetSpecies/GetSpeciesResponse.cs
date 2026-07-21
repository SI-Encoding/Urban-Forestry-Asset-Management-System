namespace UFAMS.Application.Features.Species.GetSpecies;

public sealed record GetSpeciesResponse(
    Guid Id,
    string CommonName,
    string ScientificName,
    bool IsNative);