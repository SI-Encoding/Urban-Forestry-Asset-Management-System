using UFAMS.Domain.Common;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Domain.Entities;

public class Park : BaseEntity
{
    public string Name { get; private set; }

    public GeoCoordinate Location { get; private set; }

    public double AreaInHectares { get; private set; }

    public bool IsActive { get; private set; }

    private Park()
    {
        Name = null!;
        Location = null!;
    }
    
    public Park(
        string name,
        GeoCoordinate location,
        double areaInHectares)
    {
        Name = ValidateName(name);
        Location = location ?? throw new ArgumentNullException(nameof(location));
        AreaInHectares = ValidateArea(areaInHectares);
        IsActive = true;
    }

    public void UpdateDetails(
        string name,
        GeoCoordinate location,
        double areaInHectares)
    {
        Name = ValidateName(name);
        Location = location ?? throw new ArgumentNullException(nameof(location));
        AreaInHectares = ValidateArea(areaInHectares);

        MarkUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkUpdated();
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Park name is required.",
                nameof(name));

        return name.Trim();
    }

    private static double ValidateArea(double areaInHectares)
    {
        if (areaInHectares <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(areaInHectares),
                "Area must be greater than zero.");

        return areaInHectares;
    }
}