using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.WorkOrders.CancelWorkOrder;

public sealed class CancelWorkOrderHandler
{
    private readonly IWorkOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelWorkOrderHandler(
        IWorkOrderRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CancelWorkOrderResponse> Handle(
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

        workOrder.Cancel();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new CancelWorkOrderResponse(
            workOrder.Id,
            workOrder.Status);
    }
}