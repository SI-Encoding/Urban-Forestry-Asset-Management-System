using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;

namespace UFAMS.Application.Features.WorkOrders.CreateWorkOrder;

public sealed class CreateWorkOrderHandler
{
    private readonly ITreeRepository _treeRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;


    public CreateWorkOrderHandler(
        ITreeRepository treeRepository,
        IWorkOrderRepository workOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _treeRepository = treeRepository;
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<CreateWorkOrderResponse> Handle(
        Guid treeId,
        CreateWorkOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var tree =
            await _treeRepository.GetByIdAsync(
                treeId,
                cancellationToken);


        if (tree is null)
        {
            throw new NotFoundException(
                "Tree",
                treeId);
        }


        var workOrder = new WorkOrder(
            tree,
            command.Description,
            command.DueDate);


        await _workOrderRepository.AddAsync(
            workOrder,
            cancellationToken);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return new CreateWorkOrderResponse(
            workOrder.Id,
            workOrder.TreeId,
            workOrder.Description,
            workOrder.Status,
            workOrder.DueDate);
    }
}