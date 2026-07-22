namespace UFAMS.Application.Features.Trees.FindNearbyTrees;

public sealed record FindNearbyTreesQuery(
    double Latitude,
    double Longitude,
    double RadiusMeters)
{
    public void Validate()
    {
        if (RadiusMeters <= 0)
        {
            throw new ArgumentException(
                "Radius must be greater than zero.");
        }

        if (Latitude < -90 || Latitude > 90)
        {
            throw new ArgumentException(
                "Latitude must be between -90 and 90.");
        }

        if (Longitude < -180 || Longitude > 180)
        {
            throw new ArgumentException(
                "Longitude must be between -180 and 180.");
        }
    }
}