using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.WorkOrders.GetTreeWorkOrders;

public sealed class GetTreeWorkOrdersHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetTreeWorkOrdersHandler(
        IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }


    public async Task<List<GetTreeWorkOrdersResponse>> Handle(
        GetTreeWorkOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        var workOrders =
            await _workOrderRepository.GetByTreeIdAsync(
                query.TreeId,
                cancellationToken);


        return workOrders
            .Select(workOrder =>
                new GetTreeWorkOrdersResponse(
                    workOrder.Id,
                    workOrder.TreeId,
                    workOrder.Description,
                    workOrder.Status,
                    workOrder.CreatedDate,
                    workOrder.DueDate,
                    workOrder.CompletedDate))
            .ToList();
    }
}