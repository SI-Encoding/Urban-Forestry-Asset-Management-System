using DomainSpecies = UFAMS.Domain.Entities.Species;

namespace UFAMS.Application.Interfaces;

public interface ISpeciesRepository
{
    Task<DomainSpecies?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<DomainSpecies>> GetAllAsync(
        CancellationToken cancellationToken = default);
}