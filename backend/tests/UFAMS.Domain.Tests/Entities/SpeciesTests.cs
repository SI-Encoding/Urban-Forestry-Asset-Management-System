using FluentAssertions;
using UFAMS.Domain.Entities;

namespace UFAMS.Domain.Tests.Entities;

public class SpeciesTests
{
    [Fact]
    public void Constructor_WithValidValues_CreatesSpecies()
    {
        // Arrange
        const string commonName = "Douglas Fir";
        const string scientificName = "Pseudotsuga menziesii";

        // Act
        var species = new Species(
            commonName,
            scientificName,
            true,
            "A native conifer species.");

        // Assert
        species.CommonName.Should().Be(commonName);
        species.ScientificName.Should().Be(scientificName);
        species.IsNative.Should().BeTrue();
        species.Description.Should().Be("A native conifer species.");
    }


    [Fact]
    public void Constructor_WithEmptyCommonName_ThrowsArgumentException()
    {
        // Arrange
        Action action = () => new Species(
            "",
            "Pseudotsuga menziesii",
            true);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Common name is required*");
    }


    [Fact]
    public void Constructor_WithEmptyScientificName_ThrowsArgumentException()
    {
        // Arrange
        Action action = () => new Species(
            "Douglas Fir",
            "",
            true);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Scientific name is required*");
    }


    [Fact]
    public void Constructor_WithDescription_TrimsDescription()
    {
        // Arrange
        // Act
        var species = new Species(
            "Douglas Fir",
            "Pseudotsuga menziesii",
            true,
            "  Native tree  ");

        // Assert
        species.Description.Should().Be("Native tree");
    }


    [Fact]
    public void UpdateDetails_WithValidValues_UpdatesSpecies()
    {
        // Arrange
        var species = new Species(
            "Douglas Fir",
            "Pseudotsuga menziesii",
            true,
            "Old description");

        // Act
        species.UpdateDetails(
            "Coast Douglas Fir",
            "Pseudotsuga menziesii var. menziesii",
            false,
            "Updated description");

        // Assert
        species.CommonName.Should().Be("Coast Douglas Fir");
        species.ScientificName.Should()
            .Be("Pseudotsuga menziesii var. menziesii");
        species.IsNative.Should().BeFalse();
        species.Description.Should().Be("Updated description");
    }


    [Fact]
    public void UpdateDetails_WithEmptyCommonName_ThrowsArgumentException()
    {
        // Arrange
        var species = new Species(
            "Douglas Fir",
            "Pseudotsuga menziesii",
            true);

        // Act
        Action action = () => species.UpdateDetails(
            "",
            "Pseudotsuga menziesii",
            true,
            null);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Common name is required*");
    }
}