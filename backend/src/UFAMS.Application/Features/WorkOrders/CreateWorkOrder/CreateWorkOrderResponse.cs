using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.CreateWorkOrder;

public sealed record CreateWorkOrderResponse(
    Guid Id,
    Guid TreeId,
    string Description,
    WorkOrderStatus Status,
    DateOnly? DueDate);