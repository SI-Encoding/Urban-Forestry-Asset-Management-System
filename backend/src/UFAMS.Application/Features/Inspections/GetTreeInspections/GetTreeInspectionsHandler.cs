using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Inspections.GetTreeInspections;

public sealed class GetTreeInspectionsHandler
{
    private readonly IInspectionRepository _inspectionRepository;

    public GetTreeInspectionsHandler(
        IInspectionRepository inspectionRepository)
    {
        _inspectionRepository = inspectionRepository;
    }

    public async Task<List<GetTreeInspectionsResponse>> Handle(
        GetTreeInspectionsQuery query,
        CancellationToken cancellationToken = default)
    {
        var inspections =
            await _inspectionRepository.GetByTreeIdAsync(
                query.TreeId,
                cancellationToken);

        return inspections
            .Select(i => new GetTreeInspectionsResponse(
                i.Id,
                i.TreeId,
                i.InspectionDate,
                i.ObservedHealth,
                i.Notes,
                i.Recommendation,
                i.NextInspectionDate))
            .ToList();
    }
}