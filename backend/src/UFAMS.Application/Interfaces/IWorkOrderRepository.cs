using UFAMS.Domain.Entities;

namespace UFAMS.Application.Interfaces;

public interface IWorkOrderRepository
{
    Task AddAsync(
        WorkOrder workOrder,
        CancellationToken cancellationToken = default);

    Task<WorkOrder?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<WorkOrder>> GetByTreeIdAsync(
        Guid treeId,
        CancellationToken cancellationToken = default);
}