using FluentAssertions;
using UFAMS.Domain.Entities;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Domain.Tests.Entities;

public class ParkTests
{
    [Fact]
    public void Constructor_WithValidValues_CreatesPark()
    {
        // Arrange
        var location = new GeoCoordinate(
            49.3043,
            -123.1443);

        // Act
        var park = new Park(
            "Stanley Park",
            location,
            405);

        // Assert
        park.Name.Should().Be("Stanley Park");
        park.Location.Should().Be(location);
        park.AreaInHectares.Should().Be(405);
        park.IsActive.Should().BeTrue();
    }


    [Fact]
    public void Constructor_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var location = CreateLocation();

        Action action = () => new Park(
            "",
            location,
            405);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Park name is required*");
    }


    [Fact]
    public void Constructor_WithZeroArea_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var location = CreateLocation();

        Action action = () => new Park(
            "Stanley Park",
            location,
            0);

        // Act & Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Area must be greater than zero*");
    }


    [Fact]
    public void Constructor_WithNegativeArea_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var location = CreateLocation();

        Action action = () => new Park(
            "Stanley Park",
            location,
            -10);

        // Act & Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
    }


    [Fact]
    public void Constructor_WithWhitespaceName_TrimsName()
    {
        // Arrange
        var location = CreateLocation();

        // Act
        var park = new Park(
            "  Stanley Park  ",
            location,
            405);

        // Assert
        park.Name.Should().Be("Stanley Park");
    }


    [Fact]
    public void Deactivate_WhenCalled_SetsParkInactive()
    {
        // Arrange
        var park = CreatePark();

        // Act
        park.Deactivate();

        // Assert
        park.IsActive.Should().BeFalse();
    }


    [Fact]
    public void Activate_WhenCalled_SetsParkActive()
    {
        // Arrange
        var park = CreatePark();

        park.Deactivate();

        // Act
        park.Activate();

        // Assert
        park.IsActive.Should().BeTrue();
    }


    [Fact]
    public void UpdateDetails_WithValidValues_UpdatesPark()
    {
        // Arrange
        var park = CreatePark();

        var newLocation = new GeoCoordinate(
            49.2606,
            -123.2460);

        // Act
        park.UpdateDetails(
            "Queen Elizabeth Park",
            newLocation,
            52);

        // Assert
        park.Name.Should().Be("Queen Elizabeth Park");
        park.Location.Should().Be(newLocation);
        park.AreaInHectares.Should().Be(52);
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