using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.GetTreeWorkOrders;

public sealed record GetTreeWorkOrdersResponse(
    Guid Id,
    Guid TreeId,
    string Description,
    WorkOrderStatus Status,
    DateOnly CreatedDate,
    DateOnly? DueDate,
    DateOnly? CompletedDate);