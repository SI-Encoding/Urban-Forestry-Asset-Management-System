using UFAMS.Domain.Entities;

namespace UFAMS.Application.Interfaces;

public interface IInspectionRepository
{
    Task<List<Inspection>> GetAllAsync(
        CancellationToken cancellationToken = default);
        
    Task<Inspection?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Inspection>> GetByTreeIdAsync(
        Guid treeId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Inspection inspection,
        CancellationToken cancellationToken = default);
}