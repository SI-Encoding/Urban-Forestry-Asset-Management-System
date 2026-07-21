using UFAMS.Application.Interfaces;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UFAMSDbContext _context;

    public UnitOfWork(UFAMSDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}