using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.Parks.GetParkInventory;

public sealed class GetParkInventoryHandler
{
    private readonly IParkRepository _parkRepository;
    private readonly ITreeRepository _treeRepository;


    public GetParkInventoryHandler(
        IParkRepository parkRepository,
        ITreeRepository treeRepository)
    {
        _parkRepository = parkRepository;
        _treeRepository = treeRepository;
    }


    public async Task<GetParkInventoryResponse> Handle(
        GetParkInventoryQuery query,
        CancellationToken cancellationToken = default)
    {
        var park =
            await _parkRepository.GetByIdAsync(
                query.ParkId,
                cancellationToken);


        if (park is null)
        {
            throw new NotFoundException(
                "Park",
                query.ParkId);
        }


        var trees =
            await _treeRepository.GetByParkIdAsync(
                query.ParkId,
                cancellationToken);


        var species =
            trees
                .GroupBy(t => t.Species)
                .Select(group =>
                    new SpeciesInventoryItem(
                        group.Key.CommonName,
                        group.Key.ScientificName,
                        group.Count()))
                .ToList();


        return new GetParkInventoryResponse(
            park.Id,
            park.Name,
            trees.Count,
            trees.Count(t =>
                t.HealthStatus == TreeHealthStatus.Good),
            trees.Count(t =>
                t.HealthStatus != TreeHealthStatus.Good),
            species);
    }
}