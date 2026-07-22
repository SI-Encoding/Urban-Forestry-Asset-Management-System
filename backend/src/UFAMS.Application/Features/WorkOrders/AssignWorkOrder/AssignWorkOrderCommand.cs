namespace UFAMS.Application.Features.WorkOrders.AssignWorkOrder;

public sealed record AssignWorkOrderCommand(
    Guid EmployeeId);