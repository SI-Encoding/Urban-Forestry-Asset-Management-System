using Microsoft.EntityFrameworkCore;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public class InspectionRepository : IInspectionRepository
{
    private readonly UFAMSDbContext _context;

    public InspectionRepository(
        UFAMSDbContext context)
    {
        _context = context;
    }

    public async Task<Inspection?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Inspections
            .Include(i => i.Tree)
            .ThenInclude(t => t.Species)
            .Include(i => i.Tree)
            .ThenInclude(t => t.Park)
            .FirstOrDefaultAsync(
                i => i.Id == id,
                cancellationToken);
    }

    public async Task<List<Inspection>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Inspections
            .Include(i => i.Tree)
                .ThenInclude(t => t.Species)
            .Include(i => i.Tree)
                .ThenInclude(t => t.Park)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Inspection>> GetByTreeIdAsync(
        Guid treeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Inspections
            .Where(i => i.TreeId == treeId)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Inspection inspection,
        CancellationToken cancellationToken = default)
    {
        await _context.Inspections.AddAsync(
            inspection,
            cancellationToken);
    }
}