using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.AssignWorkOrder;

public sealed record AssignWorkOrderResponse(
    Guid Id,
    Guid TreeId,
    Guid? AssignedEmployeeId,
    WorkOrderStatus Status);