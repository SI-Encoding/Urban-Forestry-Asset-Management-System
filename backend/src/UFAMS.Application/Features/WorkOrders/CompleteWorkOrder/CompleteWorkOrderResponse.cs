using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.CompleteWorkOrder;

public sealed record CompleteWorkOrderResponse(
    Guid Id,
    WorkOrderStatus Status);