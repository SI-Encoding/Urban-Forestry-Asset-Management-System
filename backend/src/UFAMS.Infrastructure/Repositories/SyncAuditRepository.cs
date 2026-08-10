using Microsoft.EntityFrameworkCore;

using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Persistence.Repositories;

public sealed class SyncAuditRepository
: ISyncAuditRepository
{
private readonly UFAMSDbContext _context;

public SyncAuditRepository(
    UFAMSDbContext context)
{
    _context = context;
}


public async Task AddAsync(
    SyncAudit audit,
    CancellationToken cancellationToken = default)
{
    await _context.SyncAudits.AddAsync(
        audit,
        cancellationToken);
}


public async Task<IReadOnlyList<SyncAudit>> GetRecentAsync(
    int count = 50,
    CancellationToken cancellationToken = default)
{
    return await _context.SyncAudits
        .AsNoTracking()
        .Include(audit => audit.Entries)
        .OrderByDescending(
            audit => audit.StartedAt)
        .Take(count)
        .ToListAsync(
            cancellationToken);
}

}
