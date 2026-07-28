using UFAMS.Domain.Entities;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Infrastructure.Persistence.SeedData;

public static class ParkSeed
{
    public static IReadOnlyList<Park> Create() =>
    [
        new Park(
            "Stanley Park",
            new GeoCoordinate(49.3043, -123.1443),
            405),

        new Park(
            "Queen Elizabeth Park",
            new GeoCoordinate(49.2415, -123.1126),
            52),

        new Park(
            "Vanier Park",
            new GeoCoordinate(49.2767, -123.1488),
            15),

        new Park(
            "Jericho Beach Park",
            new GeoCoordinate(49.2725, -123.2022),
            36),

        new Park(
            "John Hendry Park",
            new GeoCoordinate(49.2544, -123.0675),
            27),

        new Park(
            "Hinge Park",
            new GeoCoordinate(49.2702, -123.1087),
            2),

        new Park(
            "Everett Crowley Park",
            new GeoCoordinate(49.2107, -123.0348),
            38),

        new Park(
            "Pacific Spirit Regional Park",
            new GeoCoordinate(49.2615, -123.2543),
            874),

        new Park(
            "Memorial South Park",
            new GeoCoordinate(49.2218, -123.0895),
            30),

        new Park(
            "Central Park",
            new GeoCoordinate(49.2286, -123.0242),
            86)
    ];
}