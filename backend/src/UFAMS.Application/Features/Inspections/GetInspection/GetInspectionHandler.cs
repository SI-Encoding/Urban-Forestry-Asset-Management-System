using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Inspections.GetInspection;

public sealed class GetInspectionHandler
{
    private readonly IInspectionRepository _inspectionRepository;

    public GetInspectionHandler(
        IInspectionRepository inspectionRepository)
    {
        _inspectionRepository = inspectionRepository;
    }

    public async Task<GetInspectionResponse> Handle(
        GetInspectionQuery query,
        CancellationToken cancellationToken = default)
    {
        var inspection =
            await _inspectionRepository.GetByIdAsync(
                query.Id,
                cancellationToken);

        if (inspection is null)
        {
            throw new NotFoundException(
                "Inspection",
                query.Id);
        }

        return new GetInspectionResponse(
            inspection.Id,
            inspection.TreeId,
            inspection.InspectionDate,
            inspection.ObservedHealth,
            inspection.Notes,
            inspection.Recommendation,
            inspection.NextInspectionDate);
    }
}