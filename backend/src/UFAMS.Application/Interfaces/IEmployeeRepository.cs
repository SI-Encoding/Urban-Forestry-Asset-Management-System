using UFAMS.Domain.Entities;

namespace UFAMS.Application.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}