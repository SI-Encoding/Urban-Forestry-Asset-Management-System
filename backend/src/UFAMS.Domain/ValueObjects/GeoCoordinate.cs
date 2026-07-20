namespace UFAMS.Domain.ValueObjects;

public sealed record GeoCoordinate
{
    public double Latitude { get; }

    public double Longitude { get; }

    public GeoCoordinate(double latitude, double longitude)
    {
        ValidateLatitude(latitude);
        ValidateLongitude(longitude);

        Latitude = latitude;
        Longitude = longitude;
    }

    private static void ValidateLatitude(double latitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentOutOfRangeException(
                nameof(latitude),
                "Latitude must be between -90 and 90.");
        }
    }

    private static void ValidateLongitude(double longitude)
    {
        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentOutOfRangeException(
                nameof(longitude),
                "Longitude must be between -180 and 180.");
        }
    }
}