using UFAMS.Application.Common.GIS;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.FindNearbyTrees;

public sealed class FindNearbyTreesHandler
{
    private readonly ITreeRepository _treeRepository;

    public FindNearbyTreesHandler(
        ITreeRepository treeRepository)
    {
        _treeRepository = treeRepository;
    }

    public async Task<List<FindNearbyTreesResponse>> Handle(
        FindNearbyTreesQuery query,
        CancellationToken cancellationToken = default)
    {
        query.Validate();

        var trees = await _treeRepository.GetAllAsync(
            cancellationToken);

        return trees
            .Select(tree =>
            {
                var distance =
                    DistanceCalculator.Calculate(
                        query.Latitude,
                        query.Longitude,
                        tree.Location.Latitude,
                        tree.Location.Longitude);

                return new
                {
                    Tree = tree,
                    Distance = distance
                };
            })
            .Where(x =>
                x.Distance <= query.RadiusMeters)
            .OrderBy(x =>
                x.Distance)
            .Select(x =>
                new FindNearbyTreesResponse(
                    x.Tree.Id,
                    x.Tree.AssetTag,
                    x.Tree.Species.CommonName,
                    x.Tree.Park.Name,
                    x.Tree.Location.Latitude,
                    x.Tree.Location.Longitude,
                    Math.Round(x.Distance, 2)))
            .ToList();
    }
}