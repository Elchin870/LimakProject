using Limak.Domain.Entities.Common;
using System.Linq.Expressions;

namespace Limak.Application.Abstractions.Repositories.Generic;

public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    IQueryable<T> GetAll(bool ignoreQueryFilter = false);
    Task<T?> GetByIdAsync(Guid id);
    Task<int> SaveChangesAsync();
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
}
