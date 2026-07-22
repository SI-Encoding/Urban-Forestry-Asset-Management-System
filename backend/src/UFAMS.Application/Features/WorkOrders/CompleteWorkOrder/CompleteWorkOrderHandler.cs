using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.WorkOrders.CompleteWorkOrder;

public sealed class CompleteWorkOrderHandler
{
    private readonly IWorkOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteWorkOrderHandler(
        IWorkOrderRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompleteWorkOrderResponse> Handle(
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

        workOrder.Complete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new CompleteWorkOrderResponse(
            workOrder.Id,
            workOrder.Status);
    }
}