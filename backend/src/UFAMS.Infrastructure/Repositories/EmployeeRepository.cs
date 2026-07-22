using Microsoft.EntityFrameworkCore;
using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly UFAMSDbContext _context;

    public EmployeeRepository(
        UFAMSDbContext context)
    {
        _context = context;
    }


    public async Task<Employee?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(
                e => e.Id == id,
                cancellationToken);
    }
}