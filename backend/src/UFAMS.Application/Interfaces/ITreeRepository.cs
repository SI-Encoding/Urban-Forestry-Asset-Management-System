using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Application.Interfaces;

public interface ITreeRepository
{
    Task<Tree?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Tree>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<List<Tree>> SearchAsync(
        Guid? parkId,
        Guid? speciesId,
        TreeHealthStatus? healthStatus,
        double? minLatitude,
        double? maxLatitude,
        double? minLongitude,
        double? maxLongitude,
        CancellationToken cancellationToken = default);

    Task<List<Tree>> GetByParkIdAsync(
        Guid parkId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Tree tree,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string assetTag,
        CancellationToken cancellationToken = default);
}