using FluentAssertions;
using UFAMS.Domain.Entities;

namespace UFAMS.Domain.Tests.Entities;

public class EmployeeTests
{
    [Fact]
    public void Constructor_WithValidValues_CreatesEmployee()
    {
        // Arrange

        // Act
        var employee = new Employee(
            "John Smith",
            "Arborist");

        // Assert
        employee.Name.Should().Be("John Smith");
        employee.Role.Should().Be("Arborist");
    }


    [Fact]
    public void Constructor_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        Action action = () =>
            new Employee(
                "",
                "Arborist");

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Employee name is required*");
    }


    [Fact]
    public void Constructor_WithEmptyRole_ThrowsArgumentException()
    {
        // Arrange
        Action action = () =>
            new Employee(
                "John Smith",
                "");

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Employee role is required*");
    }


    [Fact]
    public void Constructor_WithWhitespaceValues_TrimsValues()
    {
        // Arrange & Act
        var employee = new Employee(
            "  John Smith  ",
            "  Arborist  ");

        // Assert
        employee.Name.Should()
            .Be("John Smith");

        employee.Role.Should()
            .Be("Arborist");
    }
}