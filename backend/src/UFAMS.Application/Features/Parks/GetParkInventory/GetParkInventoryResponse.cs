namespace UFAMS.Application.Features.Parks.GetParkInventory;

public sealed record GetParkInventoryResponse(
    Guid ParkId,
    string ParkName,
    int TotalTrees,
    int HealthyTrees,
    int TreesNeedingAttention,
    List<SpeciesInventoryItem> Species);


public sealed record SpeciesInventoryItem(
    string CommonName,
    string ScientificName,
    int Count);