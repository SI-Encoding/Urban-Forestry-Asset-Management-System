using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.GetWorkOrder;

public sealed record GetWorkOrderResponse(
    Guid Id,
    Guid TreeId,
    Guid? InspectionId,
    Guid? AssignedEmployeeId,
    string Description,
    WorkOrderStatus Status,
    DateOnly CreatedDate,
    DateOnly? DueDate,
    DateOnly? CompletedDate);