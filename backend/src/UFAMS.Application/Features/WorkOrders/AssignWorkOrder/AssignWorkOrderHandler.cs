using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.WorkOrders.AssignWorkOrder;

public sealed class AssignWorkOrderHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;


    public AssignWorkOrderHandler(
        IWorkOrderRepository workOrderRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<AssignWorkOrderResponse> Handle(
        Guid id,
        AssignWorkOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var workOrder =
            await _workOrderRepository.GetByIdAsync(
                id,
                cancellationToken);


        if (workOrder is null)
        {
            throw new NotFoundException(
                "WorkOrder",
                id);
        }


        var employee =
            await _employeeRepository.GetByIdAsync(
                command.EmployeeId,
                cancellationToken);


        if (employee is null)
        {
            throw new NotFoundException(
                "Employee",
                command.EmployeeId);
        }


        workOrder.AssignEmployee(
            employee);


        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return new AssignWorkOrderResponse(
            workOrder.Id,
            workOrder.TreeId,
            workOrder.AssignedEmployeeId,
            workOrder.Status);
    }
}