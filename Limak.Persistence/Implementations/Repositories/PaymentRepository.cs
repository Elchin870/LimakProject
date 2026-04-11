using Limak.Application.Abstractions.Repositories;
using Limak.Domain.Entities;
using Limak.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Limak.Persistence.Implementations.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly LimakDbContext _context;

    public PaymentRepository(LimakDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public IQueryable<Payment> GetAll(bool ignoreQueryFilter = false)
    {
        var query = _context.Payments.AsQueryable();

        if (ignoreQueryFilter)
            query = query.IgnoreQueryFilters();


        return query;
    }

    public async Task<Payment?> GetAsync(Expression<Func<Payment, bool>> expression)
    {
        var entity = await _context.Payments.FirstOrDefaultAsync(expression);

        return entity;
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Payments.FindAsync(id);

        return entity;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Update(Payment entity)
    {
        _context.Payments.Update(entity);
    }
}
