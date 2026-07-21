using UFAMS.Domain.Entities;

namespace UFAMS.Application.Interfaces;

public interface IParkRepository
{
    Task<Park?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<List<Park>> GetAllAsync(
        CancellationToken cancellationToken = default);
}