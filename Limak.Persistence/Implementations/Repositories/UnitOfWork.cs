using Limak.Application.Abstractions.Repositories;
using Limak.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Limak.Persistence.Implementations.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LimakDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(LimakDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();

        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
