namespace UFAMS.Application.Features.Parks.GetParks;

public sealed record GetParksResponse(
    Guid Id,
    string Name,
    double AreaInHectares);