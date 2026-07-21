using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.UpdateMeasurements;

public sealed class UpdateTreeMeasurementsHandler
{
    private readonly ITreeRepository _treeRepository;
    private readonly IUnitOfWork _unitOfWork;


    public UpdateTreeMeasurementsHandler(
        ITreeRepository treeRepository,
        IUnitOfWork unitOfWork)
    {
        _treeRepository = treeRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<UpdateTreeMeasurementsResponse> Handle(
        Guid treeId,
        UpdateTreeMeasurementsCommand command,
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


        tree.UpdateMeasurements(
            command.HeightInMeters,
            command.DiameterInCentimeters);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return new UpdateTreeMeasurementsResponse(
            tree.Id,
            tree.AssetTag,
            tree.HeightInMeters,
            tree.DiameterInCentimeters,
            tree.HealthStatus);
    }
}