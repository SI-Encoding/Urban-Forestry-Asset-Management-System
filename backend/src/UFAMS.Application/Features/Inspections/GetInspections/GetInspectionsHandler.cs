using UFAMS.Application.Features.Inspections.GetInspections;
using UFAMS.Application.Interfaces;

public sealed class GetInspectionsHandler
{
    private readonly IInspectionRepository _inspectionRepository;

    public GetInspectionsHandler(
        IInspectionRepository inspectionRepository)
    {
        _inspectionRepository = inspectionRepository;
    }

    public async Task<List<GetInspectionsResponse>> Handle(
        GetInspectionsQuery query,
        CancellationToken cancellationToken = default)
    {
        var inspections =
            await _inspectionRepository.GetAllAsync(
                cancellationToken);

        return inspections
            .Select(i => new GetInspectionsResponse(
                i.Id,
                i.TreeId,
                i.Tree.AssetTag,
                i.Tree.Species.CommonName,
                i.Tree.Park.Name,
                i.InspectionDate,
                i.ObservedHealth,
                i.Recommendation,
                i.NextInspectionDate))
            .ToList();
    }
}