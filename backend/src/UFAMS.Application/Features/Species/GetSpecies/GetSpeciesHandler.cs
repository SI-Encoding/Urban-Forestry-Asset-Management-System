using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Species.GetSpecies;

public sealed class GetSpeciesHandler
{
    private readonly ISpeciesRepository _speciesRepository;

    public GetSpeciesHandler(
        ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }

    public async Task<List<GetSpeciesResponse>> Handle(
        GetSpeciesQuery query,
        CancellationToken cancellationToken = default)
    {
        var species = await _speciesRepository.GetAllAsync(
            cancellationToken);

        return species
            .Select(s => new GetSpeciesResponse(
                s.Id,
                s.CommonName,
                s.ScientificName,
                s.IsNative))
            .ToList();
    }
}