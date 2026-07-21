using Microsoft.EntityFrameworkCore;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public sealed class ParkRepository : IParkRepository
{
    private readonly UFAMSDbContext _context;

    public ParkRepository(UFAMSDbContext context)
    {
        _context = context;
    }

    public async Task<Park?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Parks.FindAsync(
            [id],
            cancellationToken);
    }

    public async Task<List<Park>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Parks
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}