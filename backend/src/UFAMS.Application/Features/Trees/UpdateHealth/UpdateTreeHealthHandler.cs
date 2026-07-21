using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.UpdateHealth;

public sealed class UpdateTreeHealthHandler
{
    private readonly ITreeRepository _treeRepository;
    private readonly IUnitOfWork _unitOfWork;


    public UpdateTreeHealthHandler(
        ITreeRepository treeRepository,
        IUnitOfWork unitOfWork)
    {
        _treeRepository = treeRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<UpdateTreeHealthResponse> Handle(
        Guid treeId,
        UpdateTreeHealthCommand command,
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


        tree.UpdateHealth(
            command.HealthStatus);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return new UpdateTreeHealthResponse(
            tree.Id,
            tree.AssetTag,
            tree.HealthStatus);
    }
}