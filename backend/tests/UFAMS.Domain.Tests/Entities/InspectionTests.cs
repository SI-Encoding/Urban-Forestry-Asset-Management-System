using FluentAssertions;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Domain.Tests.Entities;

public class InspectionTests
{
    [Fact]
    public void Constructor_WithValidValues_CreatesInspection()
    {
        // Arrange
        var inspectionDate =
            new DateOnly(2026, 1, 1);

        var nextInspectionDate =
            new DateOnly(2027, 1, 1);

        // Act
        var inspection = new Inspection(
            Guid.NewGuid(),
            inspectionDate,
            TreeHealthStatus.Good,
            "Tree is healthy.",
            "Continue monitoring.",
            nextInspectionDate);

        // Assert
        inspection.InspectionDate.Should().Be(inspectionDate);
        inspection.ObservedHealth.Should().Be(TreeHealthStatus.Good);
        inspection.Notes.Should().Be("Tree is healthy.");
        inspection.Recommendation.Should().Be("Continue monitoring.");
        inspection.NextInspectionDate.Should().Be(nextInspectionDate);
    }


    [Fact]
    public void Constructor_WithFutureInspectionDate_ThrowsArgumentException()
    {
        // Arrange
        var futureDate =
            DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        Action action = () =>
            new Inspection(
                Guid.NewGuid(),
                futureDate,
                TreeHealthStatus.Good,
                "Notes",
                "Recommendation",
                null);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Inspection date cannot be in the future*");
    }


    [Fact]
    public void Constructor_WithEmptyNotes_ThrowsArgumentException()
    {
        // Arrange
        Action action = () =>
            new Inspection(
                Guid.NewGuid(),
                new DateOnly(2026, 1, 1),
                TreeHealthStatus.Good,
                "",
                "Recommendation",
                null);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Notes are required*");
    }


    [Fact]
    public void Constructor_WithEmptyRecommendation_ThrowsArgumentException()
    {
        // Arrange
        Action action = () =>
            new Inspection(
                Guid.NewGuid(),
                new DateOnly(2026, 1, 1),
                TreeHealthStatus.Good,
                "Notes",
                "",
                null);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Recommendation is required*");
    }


    [Fact]
    public void Constructor_WithNextInspectionBeforeInspectionDate_ThrowsArgumentException()
    {
        // Arrange
        Action action = () =>
            new Inspection(
                Guid.NewGuid(),
                new DateOnly(2026, 6, 1),
                TreeHealthStatus.Good,
                "Notes",
                "Recommendation",
                new DateOnly(2026, 5, 1));

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Next inspection date cannot be before the inspection date*");
    }


    [Fact]
    public void Constructor_WithWhitespaceNotes_TrimsNotes()
    {
        // Arrange
        var inspection = new Inspection(
            Guid.NewGuid(),
            new DateOnly(2026, 1, 1),
            TreeHealthStatus.Good,
            "  Needs pruning  ",
            "  Monitor tree  ",
            null);

        // Act & Assert
        inspection.Notes.Should()
            .Be("Needs pruning");

        inspection.Recommendation.Should()
            .Be("Monitor tree");
    }


    [Fact]
    public void UpdateNotes_WithValidNotes_UpdatesNotes()
    {
        // Arrange
        var inspection = CreateInspection();

        // Act
        inspection.UpdateNotes(
            "Updated notes");

        // Assert
        inspection.Notes.Should()
            .Be("Updated notes");
    }


    [Fact]
    public void UpdateRecommendation_WithValidRecommendation_UpdatesRecommendation()
    {
        // Arrange
        var inspection = CreateInspection();

        // Act
        inspection.UpdateRecommendation(
            "New recommendation");

        // Assert
        inspection.Recommendation.Should()
            .Be("New recommendation");
    }


    [Fact]
    public void ScheduleFollowUp_WithValidDate_UpdatesNextInspectionDate()
    {
        // Arrange
        var inspection = CreateInspection();

        var nextDate =
            new DateOnly(2027, 1, 1);

        // Act
        inspection.ScheduleFollowUp(nextDate);

        // Assert
        inspection.NextInspectionDate.Should()
            .Be(nextDate);
    }


    private static Inspection CreateInspection()
    {
        return new Inspection(
            Guid.NewGuid(),
            new DateOnly(2026, 1, 1),
            TreeHealthStatus.Good,
            "Initial notes",
            "Initial recommendation",
            null);
    }
}