using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Infrastructure.Persistence.SeedData;

public static class TreeSeed
{
    private static readonly Random Random = new(42);

    public static IReadOnlyList<Tree> Create(
        IReadOnlyList<Species> species,
        IReadOnlyList<Park> parks)
    {
        var trees = new List<Tree>();

        var speciesLookup = species.ToDictionary(
            s => s.CommonName,
            s => s);

        var assetNumber = 1;

        foreach (var park in parks)
        {
            trees.AddRange(
                CreateTreesForPark(
                    park,
                    speciesLookup,
                    ref assetNumber));
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateTreesForPark(
        Park park,
        IReadOnlyDictionary<string, Species> speciesLookup,
        ref int assetNumber)
    {
        return park.Name switch
        {
            "Stanley Park" => CreateStanleyParkTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Queen Elizabeth Park" => CreateQueenElizabethTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Vanier Park" => CreateVanierParkTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Jericho Beach Park" => CreateJerichoParkTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "John Hendry Park" => CreateJohnHendryTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Hinge Park" => CreateHingeParkTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Everett Crowley Park" => CreateEverettCrowleyTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Pacific Spirit Regional Park" => CreatePacificSpiritTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Memorial South Park" => CreateMemorialSouthTrees(
                park,
                speciesLookup,
                ref assetNumber),

            "Central Park" => CreateCentralParkTrees(
                park,
                speciesLookup,
                ref assetNumber),

            _ => []
        };
    }

    private static GeoCoordinate OffsetCoordinate(
        GeoCoordinate center,
        double maxOffset = 0.0025)
    {
        var latitude =
            center.Latitude +
            (Random.NextDouble() - 0.5) * maxOffset;

        var longitude =
            center.Longitude +
            (Random.NextDouble() - 0.5) * maxOffset;

        return new GeoCoordinate(
            latitude,
            longitude);
    }

    private static DateOnly RandomDate(
        int startYear,
        int endYear)
    {
        return new DateOnly(
            Random.Next(startYear, endYear + 1),
            Random.Next(1, 13),
            Random.Next(1, 28));
    }

    private static TreeHealthStatus RandomHealth(
        params TreeHealthStatus[] values)
    {
        return values[
            Random.Next(values.Length)];
    }

    private static Tree CreateTree(
        int assetNumber,
        Species species,
        Park park,
        TreeHealthStatus health,
        int startYear,
        int endYear,
        double minHeight,
        double maxHeight,
        double minDiameter,
        double maxDiameter)
    {
        var tree = new Tree(
            $"TREE-{assetNumber:D4}",
            species,
            park,
            OffsetCoordinate(park.Location),
            health,
            RandomDate(startYear, endYear),
            Math.Round(
                Random.NextDouble() *
                (maxHeight - minHeight) +
                minHeight,
                1),
            Math.Round(
                Random.NextDouble() *
                (maxDiameter - minDiameter) +
                minDiameter,
                1));

        tree.AssignArcGisFeatureId(
            $"ARCGIS-{assetNumber:D6}");

        return tree;
    }

    // Park-specific methods

    private static IEnumerable<Tree> CreateStanleyParkTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Douglas Fir"],
            species["Western Red Cedar"],
            species["Western Hemlock"],
            species["Sitka Spruce"]
        };

        for (int i = 0; i < 20; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair),
                    1940,
                    2010,
                    20,
                    45,
                    50,
                    180));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateQueenElizabethTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Japanese Maple"],
            species["Cherry Blossom"],
            species["Pacific Dogwood"],
            species["Red Maple"],
            species["Bigleaf Maple"]
        };

        for (int i = 0; i < 15; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair,
                        TreeHealthStatus.Poor),
                    1970,
                    2024,
                    5,
                    18,
                    10,
                    70));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateVanierParkTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Bigleaf Maple"],
            species["Red Maple"],
            species["London Plane"],
            species["Pacific Dogwood"],
            species["Cherry Blossom"]
        };

        for (int i = 0; i < 12; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair),
                    1985,
                    2023,
                    6,
                    22,
                    15,
                    80));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateJerichoParkTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Douglas Fir"],
            species["Bigleaf Maple"],
            species["Garry Oak"],
            species["Pacific Dogwood"],
            species["Red Maple"]
        };

        for (int i = 0; i < 12; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair),
                    1965,
                    2020,
                    8,
                    25,
                    20,
                    90));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateJohnHendryTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Bigleaf Maple"],
            species["Red Maple"],
            species["Vine Maple"],
            species["Douglas Fir"],
            species["London Plane"]
        };

        for (int i = 0; i < 15; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair),
                    1950,
                    2020,
                    8,
                    30,
                    20,
                    100));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateHingeParkTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Japanese Maple"],
            species["Cherry Blossom"],
            species["London Plane"],
            species["Red Maple"],
            species["Pacific Dogwood"]
        };

        for (int i = 0; i < 10; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair),
                    2010,
                    2024,
                    3,
                    12,
                    5,
                    40));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateEverettCrowleyTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Douglas Fir"],
            species["Western Red Cedar"],
            species["Western Hemlock"],
            species["Bigleaf Maple"],
            species["Vine Maple"]
        };

        for (int i = 0; i < 18; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair,
                        TreeHealthStatus.Poor),
                    1955,
                    2015,
                    12,
                    40,
                    30,
                    140));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreatePacificSpiritTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Douglas Fir"],
            species["Western Red Cedar"],
            species["Western Hemlock"],
            species["Sitka Spruce"],
            species["Bigleaf Maple"]
        };

        for (int i = 0; i < 35; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair,
                        TreeHealthStatus.Poor),
                    1930,
                    2010,
                    15,
                    50,
                    40,
                    200));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateMemorialSouthTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Red Maple"],
            species["Bigleaf Maple"],
            species["London Plane"],
            species["Cherry Blossom"],
            species["Pacific Dogwood"]
        };

        for (int i = 0; i < 10; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair),
                    1975,
                    2022,
                    5,
                    20,
                    15,
                    75));

            assetNumber++;
        }

        return trees;
    }

    private static IEnumerable<Tree> CreateCentralParkTrees(
        Park park,
        IReadOnlyDictionary<string, Species> species,
        ref int assetNumber)
    {
        var trees = new List<Tree>();

        var availableSpecies = new[]
        {
            species["Douglas Fir"],
            species["Western Red Cedar"],
            species["Bigleaf Maple"],
            species["Red Maple"],
            species["Japanese Maple"],
            species["Cherry Blossom"],
            species["London Plane"]
        };

        for (int i = 0; i < 20; i++)
        {
            var selectedSpecies =
                availableSpecies[
                    Random.Next(availableSpecies.Length)];

            trees.Add(
                CreateTree(
                    assetNumber,
                    selectedSpecies,
                    park,
                    RandomHealth(
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Good,
                        TreeHealthStatus.Fair,
                        TreeHealthStatus.Poor),
                    1960,
                    2024,
                    6,
                    30,
                    15,
                    120));

            assetNumber++;
        }

        return trees;
    }
}