using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Species.SearchSpecies;

public sealed class SearchSpeciesHandler
{
    private readonly ISpeciesRepository _speciesRepository;


    public SearchSpeciesHandler(
        ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }


    public async Task<List<SearchSpeciesResponse>> Handle(
        SearchSpeciesQuery query,
        CancellationToken cancellationToken = default)
    {
        var species =
            await _speciesRepository.GetAllAsync(
                cancellationToken);


        return species
            .Where(s =>
                s.CommonName.Contains(
                    query.SearchTerm,
                    StringComparison.OrdinalIgnoreCase)
                ||
                s.ScientificName.Contains(
                    query.SearchTerm,
                    StringComparison.OrdinalIgnoreCase))
            .Select(s =>
                new SearchSpeciesResponse(
                    s.Id,
                    s.CommonName,
                    s.ScientificName,
                    s.IsNative))
            .ToList();
    }
}