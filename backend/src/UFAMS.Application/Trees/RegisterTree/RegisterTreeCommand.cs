using UFAMS.Domain.ValueObjects;

namespace UFAMS.Application.Trees.RegisterTree;

public record RegisterTreeCommand(
    string AssetTag,
    Guid SpeciesId,
    Guid ParkId,
    GeoCoordinate Location,
    DateOnly PlantingDate,
    double HeightInMeters,
    double DiameterInCentimeters);