using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.WorkOrders.StartWorkOrder;

public sealed class StartWorkOrderHandler
{
    private readonly IWorkOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public StartWorkOrderHandler(
        IWorkOrderRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<StartWorkOrderResponse> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var workOrder =
            await _repository.GetByIdAsync(
                id,
                cancellationToken);

        if (workOrder is null)
            throw new NotFoundException(
                "WorkOrder",
                id);

        workOrder.Start();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new StartWorkOrderResponse(
            workOrder.Id,
            workOrder.Status);
    }
}