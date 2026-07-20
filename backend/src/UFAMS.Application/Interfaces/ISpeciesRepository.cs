using UFAMS.Domain.Entities;

namespace UFAMS.Application.Interfaces;

public interface ISpeciesRepository
{
    Task<Species?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}