using FluentAssertions;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

namespace UFAMS.Domain.Tests.Entities;

public class WorkOrderTests
{
    [Fact]
    public void Constructor_WithValidValues_CreatesWorkOrder()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var workOrder = new WorkOrder(
            tree,
            "Prune branches",
            null);

        // Assert
        workOrder.Tree.Should().Be(tree);
        workOrder.Description.Should().Be("Prune branches");
        workOrder.Status.Should().Be(WorkOrderStatus.Open);
        workOrder.CreatedDate.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
    }


    [Fact]
    public void Constructor_WithEmptyDescription_ThrowsArgumentException()
    {
        // Arrange
        Action action = () =>
            new WorkOrder(
                CreateTree(),
                "",
                null);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Description is required*");
    }


    [Fact]
    public void Constructor_WithWhitespaceDescription_TrimsDescription()
    {
        // Arrange
        var workOrder = new WorkOrder(
            CreateTree(),
            "  Prune branches  ",
            null);

        // Act & Assert
        workOrder.Description
            .Should()
            .Be("Prune branches");
    }


    [Fact]
    public void Constructor_WithDueDateBeforeCreatedDate_ThrowsArgumentException()
    {
        // Arrange
        var oldDate =
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

        Action action = () =>
            new WorkOrder(
                CreateTree(),
                "Prune branches",
                oldDate);

        // Act & Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Due date cannot be before creation date*");
    }


    [Fact]
    public void AssignEmployee_WithValidEmployee_AssignsEmployeeAndChangesStatus()
    {
        // Arrange
        var workOrder = CreateWorkOrder();

        var employee = new Employee(
            "John Smith",
            "Arborist");

        // Act
        workOrder.AssignEmployee(employee);

        // Assert
        workOrder.AssignedEmployee.Should().Be(employee);
        workOrder.AssignedEmployeeId.Should().Be(employee.Id);
        workOrder.Status.Should().Be(WorkOrderStatus.Assigned);
    }


    [Fact]
    public void Start_WhenAssigned_ChangesStatusToInProgress()
    {
        // Arrange
        var workOrder = CreateWorkOrder();

        workOrder.AssignEmployee(
            new Employee(
                "John Smith",
                "Arborist"));

        // Act
        workOrder.Start();

        // Assert
        workOrder.Status.Should()
            .Be(WorkOrderStatus.InProgress);
    }


    [Fact]
    public void Complete_WhenInProgress_ChangesStatusToCompleted()
    {
        // Arrange
        var workOrder = CreateWorkOrder();

        workOrder.AssignEmployee(
            new Employee(
                "John Smith",
                "Arborist"));

        workOrder.Start();

        // Act
        workOrder.Complete();

        // Assert
        workOrder.Status.Should()
            .Be(WorkOrderStatus.Completed);

        workOrder.CompletedDate.Should()
            .NotBeNull();
    }


    [Fact]
    public void Cancel_WhenOpen_ChangesStatusToCancelled()
    {
        // Arrange
        var workOrder = CreateWorkOrder();

        // Act
        workOrder.Cancel();

        // Assert
        workOrder.Status.Should()
            .Be(WorkOrderStatus.Cancelled);
    }


    [Fact]
    public void Start_WhenNotAssigned_ThrowsInvalidOperationException()
    {
        // Arrange
        var workOrder = CreateWorkOrder();

        Action action = () =>
            workOrder.Start();

        // Act & Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Only assigned work orders can be started*");
    }


    [Fact]
    public void Cancel_WhenCompleted_ThrowsInvalidOperationException()
    {
        // Arrange
        var workOrder = CreateWorkOrder();

        workOrder.AssignEmployee(
           new Employee(
                "John Smith",
                "Arborist"));

        workOrder.Start();

        workOrder.Complete();

        Action action = () =>
            workOrder.Cancel();

        // Act & Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Completed work orders cannot be cancelled*");
    }


    private static WorkOrder CreateWorkOrder()
    {
        return new WorkOrder(
            CreateTree(),
            "Prune branches",
            null);
    }


    private static Tree CreateTree()
    {
        var species =
            new Species(
                "Douglas Fir",
                "Pseudotsuga menziesii",
                true);

        var park =
            new Park(
                "Queen Elizabeth Park",
                new GeoCoordinate(
                    49.2415,
                    -123.1126),
                52);

        return new Tree(
            "TREE-001",
            species,
            park,
            new GeoCoordinate(
                49.2415,
                -123.1126),
            TreeHealthStatus.Good,
            new DateOnly(2020, 1, 1),
            12,
            30);
    }
}