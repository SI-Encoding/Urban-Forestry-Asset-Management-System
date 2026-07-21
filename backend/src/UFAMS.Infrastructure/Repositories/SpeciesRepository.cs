using Microsoft.EntityFrameworkCore;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly UFAMSDbContext _context;

    public SpeciesRepository(UFAMSDbContext context)
    {
        _context = context;
    }

    public async Task<Species?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Species.FirstOrDefaultAsync(
            s => s.Id == id,
            cancellationToken);
    }

    public async Task<List<Species>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Species
            .OrderBy(s => s.CommonName)
            .ToListAsync(cancellationToken);
    }

}