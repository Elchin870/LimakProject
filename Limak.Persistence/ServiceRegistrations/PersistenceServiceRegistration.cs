using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.ServiceRegistrations;
using Limak.Domain.Entities;
using Limak.Persistence.Abstractions;
using Limak.Persistence.ContextInitalizers;
using Limak.Persistence.Contexts;
using Limak.Persistence.Implementations.Repositories;
using Limak.Persistence.Implementations.Services;
using Limak.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Limak.Persistence.ServiceRegistrations;

public static class PersistenceServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<LimakDbContext>(opt =>
        {
            opt.UseSqlServer(conn);
        });

        services.AddHttpClient();

        services.AddScoped<IContextInitalizer, DbContextInitalizer>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IOfficeService, OfficeService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<IShopService, ShopService>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IShipmentTypeRepository, ShipmentTypeRepository>();
        services.AddScoped<IShipmentTypeService, ShipmentTypeService>();
        services.AddScoped<IDeliveryTypeRepository, DeliveryTypeRepository>();
        services.AddScoped<IDeliveryTypeService, DeliveryTypeService>();
        services.AddScoped<ITarifRepository, TariffRepository>();
        services.AddScoped<ITariffService, TariffService>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ICountryAddressRepository, CountryAddressRepository>();
        services.AddScoped<ICountryAddressService, CountryAddressService>();
        services.AddScoped<IKargomatRepository, KargomatRepository>();
        services.AddScoped<IKargomatService, KargomatService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IBalanceTransactionRepository, BalanceTransactionRepository>();
        services.AddScoped<IBalanceTransactionService, BalanceTransactionService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<IPackageService, PackageService>();


        services.AddIdentity<AppUser, AppRole>(opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireDigit = true;
            opt.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<LimakDbContext>().AddDefaultTokenProviders();

        services.AddAutoMapper(_ => { }, typeof(PersistenceServiceRegistration).Assembly);

        services.AddScoped<BaseAuditableInterceptor>();
    }
}
