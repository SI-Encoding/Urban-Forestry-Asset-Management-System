using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;

namespace UFAMS.Application.Features.Inspections.CreateInspection;

public sealed class CreateInspectionHandler
{
    private readonly ITreeRepository _treeRepository;
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInspectionHandler(
        ITreeRepository treeRepository,
        IInspectionRepository inspectionRepository,
        IUnitOfWork unitOfWork)
    {
        _treeRepository = treeRepository;
        _inspectionRepository = inspectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateInspectionResponse> Handle(
        Guid treeId,
        CreateInspectionCommand command,
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

        var inspection = new Inspection(
            treeId,
            command.InspectionDate,
            command.ObservedHealth,
            command.Notes,
            command.Recommendation,
            command.NextInspectionDate);

        await _inspectionRepository.AddAsync(
            inspection,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new CreateInspectionResponse(
            inspection.Id,
            inspection.TreeId,
            inspection.InspectionDate,
            inspection.ObservedHealth,
            inspection.Notes,
            inspection.Recommendation,
            inspection.NextInspectionDate);
    }
}