using UFAMS.Domain.Entities;

namespace UFAMS.Application.Interfaces;

public interface ITreeRepository
{
    Task<Tree?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Tree>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Tree tree,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string assetTag,
        CancellationToken cancellationToken = default);
}