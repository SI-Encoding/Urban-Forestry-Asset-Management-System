using UFAMS.Domain.Common;
using UFAMS.Domain.Enums;

namespace UFAMS.Domain.Entities;

public class WorkOrder : BaseEntity
{
    public Guid TreeId { get; private set; }

    public Tree Tree { get; private set; } = null!;


    public Guid? InspectionId { get; private set; }

    public Inspection? Inspection { get; private set; }


    public Guid? AssignedEmployeeId { get; private set; }

    public Employee? AssignedEmployee { get; private set; }


    public string Description { get; private set; } = string.Empty;

    public WorkOrderStatus Status { get; private set; }

    public DateOnly CreatedDate { get; private set; }

    public DateOnly? DueDate { get; private set; }

    public DateOnly? CompletedDate { get; private set; }


    private WorkOrder()
    {
    }


    public WorkOrder(
        Tree tree,
        string description,
        DateOnly? dueDate,
        Inspection? inspection = null)
    {
        Tree = tree ??
            throw new ArgumentNullException(nameof(tree));

        TreeId = tree.Id;

        Inspection = inspection;

        InspectionId = inspection?.Id;

        Description = ValidateDescription(
            description);

        CreatedDate = DateOnly.FromDateTime(
            DateTime.UtcNow);

        DueDate = ValidateDueDate(
            CreatedDate,
            dueDate);

        Status = WorkOrderStatus.Open;
    }


    public void AssignEmployee(
        Employee employee)
    {
        if (Status == WorkOrderStatus.Completed ||
            Status == WorkOrderStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "Cannot assign a closed work order.");
        }

        AssignedEmployee =
            employee ??
            throw new ArgumentNullException(nameof(employee));

        AssignedEmployeeId =
            employee.Id;

        Status = WorkOrderStatus.Assigned;

        MarkUpdated();
    }


    public void Start()
{
    if (Status != WorkOrderStatus.Open &&
        Status != WorkOrderStatus.Assigned)
    {
        throw new InvalidOperationException(
            "Only open or assigned work orders can be started.");
    }

    Status = WorkOrderStatus.InProgress;

    MarkUpdated();
}


    public void Complete()
    {
        if (Status != WorkOrderStatus.InProgress)
        {
            throw new InvalidOperationException(
                "Only in-progress work orders can be completed.");
        }

        Status = WorkOrderStatus.Completed;

        CompletedDate =
            DateOnly.FromDateTime(
                DateTime.UtcNow);

        MarkUpdated();
    }


    public void Cancel()
    {
        if (Status == WorkOrderStatus.Completed)
        {
            throw new InvalidOperationException(
                "Completed work orders cannot be cancelled.");
        }

        Status = WorkOrderStatus.Cancelled;

        MarkUpdated();
    }


    private static string ValidateDescription(
        string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Description is required.",
                nameof(description));
        }

        return description.Trim();
    }


    private static DateOnly? ValidateDueDate(
        DateOnly createdDate,
        DateOnly? dueDate)
    {
        if (dueDate.HasValue &&
            dueDate.Value < createdDate)
        {
            throw new ArgumentException(
                "Due date cannot be before creation date.",
                nameof(dueDate));
        }

        return dueDate;
    }
}