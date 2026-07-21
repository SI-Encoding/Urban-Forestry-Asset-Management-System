using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Parks.GetParks;

public sealed class GetParksHandler
{
    private readonly IParkRepository _parkRepository;

    public GetParksHandler(
        IParkRepository parkRepository)
    {
        _parkRepository = parkRepository;
    }

    public async Task<List<GetParksResponse>> Handle(
        GetParksQuery query,
        CancellationToken cancellationToken = default)
    {
        var parks = await _parkRepository.GetAllAsync(
            cancellationToken);

        return parks
            .Select(p => new GetParksResponse(
                p.Id,
                p.Name,
                p.AreaInHectares))
            .ToList();
    }
}