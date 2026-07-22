using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.StartWorkOrder;

public sealed record StartWorkOrderResponse(
    Guid Id,
    WorkOrderStatus Status);