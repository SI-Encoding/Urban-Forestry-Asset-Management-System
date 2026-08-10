using Microsoft.EntityFrameworkCore.Storage;
using UFAMS.Application.Interfaces;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UFAMSDbContext _context;

    private IDbContextTransaction? _transaction;

    public UnitOfWork(
        UFAMSDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            return;
        }

        _transaction =
            await _context.Database.BeginTransactionAsync(
                cancellationToken);
    }

    public async Task CommitTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.CommitAsync(
            cancellationToken);

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollbackTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.RollbackAsync(
            cancellationToken);

        await _transaction.DisposeAsync();

        _transaction = null;
    }
}