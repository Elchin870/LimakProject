using Limak.Application.Abstractions.Repositories.Generic;
using Limak.Domain.Entities.Common;
using Limak.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Limak.Persistence.Implementations.Repositories.Generic;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly LimakDbContext _context;

    public Repository(LimakDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public IQueryable<T> GetAll(bool ignoreQueryFilter = false)
    {

        var query = _context.Set<T>().AsQueryable();

        if (ignoreQueryFilter)
            query = query.IgnoreQueryFilters();


        return query;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        return entity;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }


    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(expression);
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        var entity = await _context.Set<T>().AnyAsync(expression);

        return entity;
    }
}
