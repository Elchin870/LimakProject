using Limak.Domain.Entities;
using Limak.Domain.Entities.Common;
using Limak.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Limak.Persistence.Contexts;

public class LimakDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    private readonly BaseAuditableInterceptor _interceptor;
    public LimakDbContext(DbContextOptions<LimakDbContext> options, BaseAuditableInterceptor interceptor) : base(options)
    {
        _interceptor = interceptor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<Office>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Shop>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Announcement>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Partner>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Tariff>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<CountryAddress>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Kargomat>().HasQueryFilter(x => !x.IsDeleted);
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptor);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Office> Offices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Partner> Partners { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<ShipmentType> ShipmentTypes { get; set; }
    public DbSet<DeliveryType> DeliveryTypes { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<CountryAddress> CountryAdresses { get; set; }
    public DbSet<Kargomat> Kargomats { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<BalanceTransaction> BalanceTransactions { get; set; }
    public DbSet<Package> Packages { get; set; }
}
