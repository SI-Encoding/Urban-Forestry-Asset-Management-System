using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.WorkOrders.GetWorkOrders;

public sealed class GetWorkOrdersHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;


    public GetWorkOrdersHandler(
        IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }


    public async Task<List<GetWorkOrdersResponse>> Handle(
        GetWorkOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        var workOrders =
            await _workOrderRepository.GetAllAsync(
                cancellationToken);


        return workOrders
            .Select(workOrder =>
                new GetWorkOrdersResponse(
                    workOrder.Id,
                    workOrder.TreeId,
                    workOrder.Tree.AssetTag,
                    workOrder.Tree.Species.CommonName,
                    workOrder.Tree.Park.Name,
                    workOrder.AssignedEmployeeId,
                    workOrder.AssignedEmployee == null
                        ? null
                        : workOrder.AssignedEmployee.Name,
                    workOrder.Description,
                    workOrder.Status,
                    workOrder.CreatedDate,
                    workOrder.DueDate,
                    workOrder.CompletedDate
                ))
            .ToList();
    }
}