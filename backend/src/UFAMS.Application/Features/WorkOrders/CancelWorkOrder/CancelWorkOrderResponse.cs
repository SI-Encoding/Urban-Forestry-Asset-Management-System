using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.CancelWorkOrder;

public sealed record CancelWorkOrderResponse(
    Guid Id,
    WorkOrderStatus Status);