using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.SearchTrees;

public sealed class SearchTreesHandler
{
    private readonly ITreeRepository _repository;

    public SearchTreesHandler(
        ITreeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SearchTreesResponse>> Handle(
        SearchTreesQuery query,
        CancellationToken cancellationToken = default)
    {
        var trees = await _repository.SearchAsync(
            query.ParkId,
            query.SpeciesId,
            query.HealthStatus,
            query.MinLatitude,
            query.MaxLatitude,
            query.MinLongitude,
            query.MaxLongitude,
            cancellationToken);

        return trees.Select(tree =>
            new SearchTreesResponse(
                tree.Id,
                tree.AssetTag,
                tree.Species.CommonName,
                tree.Park.Name,
                tree.Location.Latitude,
                tree.Location.Longitude,
                tree.HealthStatus.ToString()))
            .ToList();
    }
}