using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Inspections.ScheduleFollowUp;

public sealed class ScheduleFollowUpHandler
{
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleFollowUpHandler(
        IInspectionRepository inspectionRepository,
        IUnitOfWork unitOfWork)
    {
        _inspectionRepository = inspectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ScheduleFollowUpResponse> Handle(
        Guid id,
        ScheduleFollowUpCommand command,
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

        inspection.ScheduleFollowUp(
            command.NextInspectionDate);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new ScheduleFollowUpResponse(
            inspection.Id,
            inspection.TreeId,
            inspection.NextInspectionDate);
    }
}