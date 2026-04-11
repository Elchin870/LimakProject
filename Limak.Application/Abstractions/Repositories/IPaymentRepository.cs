using Limak.Domain.Entities;
using System.Linq.Expressions;

namespace Limak.Application.Abstractions.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
    Task<int> SaveChangesAsync();
    Task<Payment?> GetAsync(Expression<Func<Payment, bool>> expression);
    Task<Payment?> GetByIdAsync(Guid id);
    void Update(Payment entity);
    IQueryable<Payment> GetAll(bool ignoreQueryFilter = false);
}
