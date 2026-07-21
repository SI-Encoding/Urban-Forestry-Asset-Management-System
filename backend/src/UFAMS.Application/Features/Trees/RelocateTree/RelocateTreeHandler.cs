using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.RelocateTree;

public sealed class RelocateTreeHandler
{
    private readonly ITreeRepository _treeRepository;
    private readonly IParkRepository _parkRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RelocateTreeHandler(
        ITreeRepository treeRepository,
        IParkRepository parkRepository,
        IUnitOfWork unitOfWork)
    {
        _treeRepository = treeRepository;
        _parkRepository = parkRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<RelocateTreeResponse> Handle(
        Guid treeId,
        RelocateTreeCommand command,
        CancellationToken cancellationToken = default)
    {
        var tree = await _treeRepository.GetByIdAsync(
            treeId,
            cancellationToken);


        if (tree is null)
        {
            throw new NotFoundException(
                "Tree",
                treeId);
        }


        var park = await _parkRepository.GetByIdAsync(
            command.ParkId,
            cancellationToken);


        if (park is null)
        {
            throw new NotFoundException(
                "Park",
                command.ParkId);
        }


        tree.Relocate(
            park,
            command.Location);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return new RelocateTreeResponse(
            tree.Id,
            tree.AssetTag,
            tree.Park.Name,
            tree.Location);
    }
}