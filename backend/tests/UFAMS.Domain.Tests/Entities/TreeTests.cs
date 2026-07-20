using FluentAssertions;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Domain.Tests.Entities;

public class TreeTests
{
    [Fact]
    public void Constructor_WithValidValues_CreatesTree()
    {
        // Arrange
        var species = CreateSpecies();
        var park = CreatePark();
        var location = CreateLocation();

        var plantingDate = new DateOnly(2020, 5, 15);

        // Act
        var tree = new Tree(
            "TREE-001",
            species,
            park,
            location,
            TreeHealthStatus.Good,
            plantingDate,
            15.5,
            35);

        // Assert
        tree.AssetTag.Should().Be("TREE-001");
        tree.Species.Should().Be(species);
        tree.Park.Should().Be(park);
        tree.Location.Should().Be(location);
        tree.HealthStatus.Should().Be(TreeHealthStatus.Good);
        tree.PlantingDate.Should().Be(plantingDate);
        tree.HeightInMeters.Should().Be(15.5);
        tree.DiameterInCentimeters.Should().Be(35);
    }


    [Fact]
    public void Constructor_WithEmptyAssetTag_ThrowsArgumentException()
    {
        // Arrange
        Action action = () => new Tree(
            "",
            CreateSpecies(),
            CreatePark(),
            CreateLocation(),
            TreeHealthStatus.Good,
            new DateOnly(2020, 5, 15),
            15,
            30);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Asset tag is required*");
    }


    [Fact]
    public void Constructor_WithFuturePlantingDate_ThrowsArgumentException()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(
            DateTime.Today.AddDays(1));

        Action action = () => new Tree(
            "TREE-001",
            CreateSpecies(),
            CreatePark(),
            CreateLocation(),
            TreeHealthStatus.Good,
            futureDate,
            15,
            30);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Planting date cannot be in the future*");
    }


    [Fact]
    public void Constructor_WithNegativeHeight_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Action action = () => new Tree(
            "TREE-001",
            CreateSpecies(),
            CreatePark(),
            CreateLocation(),
            TreeHealthStatus.Good,
            new DateOnly(2020, 5, 15),
            -1,
            30);

        // Act & Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
    }


    [Fact]
    public void UpdateMeasurements_WithValidValues_UpdatesMeasurements()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        tree.UpdateMeasurements(
            20,
            45);

        // Assert
        tree.HeightInMeters.Should().Be(20);
        tree.DiameterInCentimeters.Should().Be(45);
    }


    [Fact]
    public void UpdateHealth_WithNewStatus_UpdatesHealthStatus()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        tree.UpdateHealth(TreeHealthStatus.Excellent);

        // Assert
        tree.HealthStatus.Should()
            .Be(TreeHealthStatus.Excellent);
    }


    [Fact]
    public void Relocate_WithNewLocation_UpdatesTreeLocation()
    {
        // Arrange
        var tree = CreateTree();

        var newPark = new Park(
            "Queen Elizabeth Park",
            new GeoCoordinate(
                49.2400,
                -123.1120),
            52);

        var newLocation = new GeoCoordinate(
            49.2401,
            -123.1121);

        // Act
        tree.Relocate(
            newPark,
            newLocation);

        // Assert
        tree.Park.Should().Be(newPark);
        tree.Location.Should().Be(newLocation);
    }


    private static Tree CreateTree()
    {
        return new Tree(
            "TREE-001",
            CreateSpecies(),
            CreatePark(),
            CreateLocation(),
            TreeHealthStatus.Good,
            new DateOnly(2020, 5, 15),
            15,
            30);
    }


    private static Species CreateSpecies()
    {
        return new Species(
            "Douglas Fir",
            "Pseudotsuga menziesii",
            true);
    }


    private static Park CreatePark()
    {
        return new Park(
            "Stanley Park",
            CreateLocation(),
            405);
    }


    private static GeoCoordinate CreateLocation()
    {
        return new GeoCoordinate(
            49.3043,
            -123.1443);
    }
}