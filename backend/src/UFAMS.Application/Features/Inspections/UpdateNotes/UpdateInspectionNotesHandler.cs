using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Inspections.UpdateNotes;

public sealed class UpdateInspectionNotesHandler
{
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInspectionNotesHandler(
        IInspectionRepository inspectionRepository,
        IUnitOfWork unitOfWork)
    {
        _inspectionRepository = inspectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateInspectionNotesResponse> Handle(
        Guid id,
        UpdateInspectionNotesCommand command,
        CancellationToken cancellationToken = default)
    {
        var inspection =
            await _inspectionRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (inspection is null)
        {
            throw new NotFoundException(
                "Inspection",
                id);
        }

        inspection.UpdateNotes(
            command.Notes);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new UpdateInspectionNotesResponse(
            inspection.Id,
            inspection.TreeId,
            inspection.Notes);
    }
}