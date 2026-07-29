using UFAMS.Domain.Enums;

namespace UFAMS.Application.Features.WorkOrders.GetWorkOrders;

public sealed record GetWorkOrdersResponse(
    Guid Id,
    Guid TreeId,
    string AssetTag,
    string SpeciesName,
    string ParkName,
    Guid? AssignedEmployeeId,
    string? AssignedEmployeeName,
    string Description,
    WorkOrderStatus Status,
    DateOnly CreatedDate,
    DateOnly? DueDate,
    DateOnly? CompletedDate
);