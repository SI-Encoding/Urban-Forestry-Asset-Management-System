using FluentAssertions;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Domain.Tests.ValueObjects;

public class GeoCoordinateTests
{
    [Fact]
    public void Constructor_WithValidCoordinates_CreatesGeoCoordinate()
    {
        // Arrange
        const double latitude = 49.2827;
        const double longitude = -123.1207;

        // Act
        var coordinate = new GeoCoordinate(latitude, longitude);

        // Assert
        coordinate.Latitude.Should().Be(latitude);
        coordinate.Longitude.Should().Be(longitude);
    }

    [Fact]
    public void Constructor_WithLatitudeGreaterThan90_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var action = () => new GeoCoordinate(91, 0);

        // Act & Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithLongitudeGreaterThan180_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var action = () => new GeoCoordinate(0, 181);

        // Act & Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithLatitudeLessThanMinus90_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var action = () => new GeoCoordinate(-91, 0);

        // Act & Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithLongitudeLessThanMinus180_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var action = () => new GeoCoordinate(0, -181);

        // Act & Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TwoCoordinates_WithSameValues_AreEqual()
    {
        // Arrange
        var first = new GeoCoordinate(49.2827, -123.1207);

        var second = new GeoCoordinate(49.2827, -123.1207);

        // Assert
        first.Should().Be(second);
    }

    [Fact]
    public void TwoCoordinates_WithDifferentValues_AreNotEqual()
    {
        // Arrange
        var first = new GeoCoordinate(
            49.2827,
            -123.1207);

        var second = new GeoCoordinate(
            49.3000,
            -123.1207);

        // Assert
        first.Should().NotBe(second);
    }

    [Fact]
    public void Constructor_WithBoundaryCoordinates_CreatesGeoCoordinate()
    {
        // Arrange & Act
        var coordinate = new GeoCoordinate(
            90,
            180);

        // Assert
        coordinate.Latitude.Should().Be(90);
        coordinate.Longitude.Should().Be(180);
    }
    
}