using Microsoft.EntityFrameworkCore;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly UFAMSDbContext _context;


    public WorkOrderRepository(
        UFAMSDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        WorkOrder workOrder,
        CancellationToken cancellationToken = default)
    {
        await _context.WorkOrders.AddAsync(
            workOrder,
            cancellationToken);
    }


    public async Task<WorkOrder?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .Include(w => w.Tree)
            .Include(w => w.Inspection)
            .Include(w => w.AssignedEmployee)
            .FirstOrDefaultAsync(
                w => w.Id == id,
                cancellationToken);
    }


    public async Task<List<WorkOrder>> GetByTreeIdAsync(
        Guid treeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .Where(w => w.TreeId == treeId)
            .OrderByDescending(w => w.CreatedDate)
            .ToListAsync(cancellationToken);
    }
}