namespace UFAMS.Application.Features.WorkOrders.CreateWorkOrder;

public sealed record CreateWorkOrderCommand(
    string Description,
    DateOnly? DueDate);