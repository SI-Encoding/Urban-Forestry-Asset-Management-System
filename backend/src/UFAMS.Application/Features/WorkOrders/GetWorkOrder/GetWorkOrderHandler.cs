using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.WorkOrders.GetWorkOrder;

public sealed class GetWorkOrderHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetWorkOrderHandler(
        IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<GetWorkOrderResponse> Handle(
        GetWorkOrderQuery query,
        CancellationToken cancellationToken = default)
    {
        var workOrder =
            await _workOrderRepository.GetByIdAsync(
                query.Id,
                cancellationToken);

        if (workOrder is null)
        {
            throw new NotFoundException(
                "WorkOrder",
                query.Id);
        }

        return new GetWorkOrderResponse(
            workOrder.Id,
            workOrder.TreeId,
            workOrder.InspectionId,
            workOrder.AssignedEmployeeId,
            workOrder.Description,
            workOrder.Status,
            workOrder.CreatedDate,
            workOrder.DueDate,
            workOrder.CompletedDate);
    }
}