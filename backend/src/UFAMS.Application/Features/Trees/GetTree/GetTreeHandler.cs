using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.GetTree;

public sealed class GetTreeHandler
{
    private readonly ITreeRepository _treeRepository;

    public GetTreeHandler(
        ITreeRepository treeRepository)
    {
        _treeRepository = treeRepository;
    }

    public async Task<GetTreeResponse> Handle(
        GetTreeQuery query,
        CancellationToken cancellationToken = default)
    {
        var tree = await _treeRepository.GetByIdAsync(
            query.Id,
            cancellationToken);

        if (tree is null)
        {
            throw new NotFoundException(
                "Tree",
                query.Id);
        }

        return new GetTreeResponse(
            tree.Id,
            tree.AssetTag,
            tree.Species.CommonName,
            tree.Park.Name,
            tree.Location,
            tree.HealthStatus);
    }
}