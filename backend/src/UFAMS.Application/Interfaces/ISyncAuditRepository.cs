using UFAMS.Domain.Entities;

namespace UFAMS.Application.Interfaces;

public interface ISyncAuditRepository
{
Task AddAsync(
SyncAudit audit,
CancellationToken cancellationToken = default);

Task<IReadOnlyList<SyncAudit>> GetRecentAsync(
    int count = 50,
    CancellationToken cancellationToken = default);

}
