using UFAMS.Domain.Common;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Domain.Entities;

public class Tree : BaseEntity
{
    public string AssetTag { get; private set; }

    public Guid SpeciesId { get; private set; }

    public Species Species { get; private set; } = null!;

    public Guid ParkId { get; private set; }

    public Park Park { get; private set; } = null!;

    public GeoCoordinate Location { get; private set; }

    public TreeHealthStatus HealthStatus { get; private set; }

    public DateOnly PlantingDate { get; private set; }

    public double HeightInMeters { get; private set; }

    public double DiameterInCentimeters { get; private set; }

    private Tree()
    {
        AssetTag = null!;
        Species = null!;
        Park = null!;
        Location = null!;
    }
    public Tree(
        string assetTag,
        Species species,
        Park park,
        GeoCoordinate location,
        TreeHealthStatus healthStatus,
        DateOnly plantingDate,
        double heightInMeters,
        double diameterInCentimeters)
    {
        AssetTag = ValidateAssetTag(assetTag);

        Species = species ?? throw new ArgumentNullException(nameof(species));
        Park = park ?? throw new ArgumentNullException(nameof(park));
        Location = location ?? throw new ArgumentNullException(nameof(location));

        HealthStatus = healthStatus;
        PlantingDate = ValidatePlantingDate(plantingDate);

        HeightInMeters = ValidateHeight(heightInMeters);
        DiameterInCentimeters = ValidateDiameter(diameterInCentimeters);
    }

    public void UpdateMeasurements(
        double heightInMeters,
        double diameterInCentimeters)
    {
        HeightInMeters = ValidateHeight(heightInMeters);
        DiameterInCentimeters = ValidateDiameter(diameterInCentimeters);

        MarkUpdated();
    }

    public void UpdateHealth(TreeHealthStatus healthStatus)
    {
        HealthStatus = healthStatus;

        MarkUpdated();
    }

    public void Relocate(
        Park park,
        GeoCoordinate location)
    {
        Park = park ?? throw new ArgumentNullException(nameof(park));
        Location = location ?? throw new ArgumentNullException(nameof(location));

        MarkUpdated();
    }

    private static string ValidateAssetTag(string assetTag)
    {
        if (string.IsNullOrWhiteSpace(assetTag))
            throw new ArgumentException(
                "Asset tag is required.",
                nameof(assetTag));

        return assetTag.Trim().ToUpperInvariant();
    }

    private static DateOnly ValidatePlantingDate(DateOnly plantingDate)
    {
        if (plantingDate > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException(
                "Planting date cannot be in the future.",
                nameof(plantingDate));

        return plantingDate;
    }

    private static double ValidateHeight(double height)
    {
        if (height < 0)
            throw new ArgumentOutOfRangeException(
                nameof(height),
                "Height cannot be negative.");

        return height;
    }

    private static double ValidateDiameter(double diameter)
    {
        if (diameter < 0)
            throw new ArgumentOutOfRangeException(
                nameof(diameter),
                "Diameter cannot be negative.");

        return diameter;
    }
}