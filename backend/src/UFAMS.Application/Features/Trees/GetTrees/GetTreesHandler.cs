using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.GetTrees;

public sealed class GetTreesHandler
{
    private readonly ITreeRepository _treeRepository;

    public GetTreesHandler(
        ITreeRepository treeRepository)
    {
        _treeRepository = treeRepository;
    }

    public async Task<List<GetTreesResponse>> Handle(
        GetTreesQuery query,
        CancellationToken cancellationToken = default)
    {
        var trees = await _treeRepository.GetAllAsync(
            cancellationToken);

        return trees
            .Select(tree => new GetTreesResponse(
                tree.Id,
                tree.AssetTag,
                tree.Species.CommonName,
                tree.Park.Name,
                tree.Location,
                tree.HealthStatus))
            .ToList();
    }
}