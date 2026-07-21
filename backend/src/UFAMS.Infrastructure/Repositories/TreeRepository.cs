using Microsoft.EntityFrameworkCore;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public class TreeRepository : ITreeRepository
{
    private readonly UFAMSDbContext _context;

    public TreeRepository(UFAMSDbContext context)
    {
        _context = context;
    }

    public async Task<Tree?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Trees
            .Include(t => t.Species)
            .Include(t => t.Park)
            .FirstOrDefaultAsync(
                t => t.Id == id,
                cancellationToken);
    }

    public async Task<List<Tree>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Trees
            .Include(t => t.Species)
            .Include(t => t.Park)
            .OrderBy(t => t.AssetTag)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Tree>> SearchAsync(
        Guid? parkId,
        Guid? speciesId,
        TreeHealthStatus? healthStatus,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Trees
            .Include(t => t.Species)
            .Include(t => t.Park)
            .AsQueryable();

        if (parkId.HasValue)
        {
            query = query.Where(t => t.Park.Id == parkId.Value);
        }

        if (speciesId.HasValue)
        {
            query = query.Where(t => t.Species.Id == speciesId.Value);
        }

        if (healthStatus.HasValue)
        {
            query = query.Where(t => t.HealthStatus == healthStatus.Value);
        }

        return await query
            .OrderBy(t => t.AssetTag)
            .ToListAsync(cancellationToken);
    }
    public async Task AddAsync(
        Tree tree,
        CancellationToken cancellationToken = default)
    {
        await _context.Trees.AddAsync(
            tree,
            cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string assetTag,
        CancellationToken cancellationToken = default)
    {
        return await _context.Trees.AnyAsync(
            t => t.AssetTag == assetTag,
            cancellationToken);
    }
}