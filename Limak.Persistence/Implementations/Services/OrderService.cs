using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.OrderDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Application.Helpers;
using Limak.Domain.Entities;
using Limak.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBalanceTransactionRepository _balanceTransactionRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPackageRepository _packageRepository;
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;
    public OrderService(IOrderRepository orderRepository, IBalanceTransactionRepository balanceTransactionRepository, INotificationRepository notificationRepository, ICustomerRepository customerRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork, IPackageRepository packageRepository, IEmailService emailService, UserManager<AppUser> userManager)
    {
        _orderRepository = orderRepository;
        _balanceTransactionRepository = balanceTransactionRepository;
        _notificationRepository = notificationRepository;
        _customerRepository = customerRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _packageRepository = packageRepository;
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<ResultDto> CreateAsync(Guid customerId, UserOrderCreateDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new NotFoundException("Customer is not found");
            }

            if (customer.Balance < dto.PriceAzn)
            {
                throw new NotEnoughBalanceException("Balans yetərli deyil!");
            }

            var order = new Order
            {
                CustomerId = customerId,
                CountryId = dto.CountryId,
                StoreName = dto.StoreName,
                StoreUrl = dto.StoreUrl,
                Quantity = dto.Quantity,
                PriceAzn = dto.PriceAzn,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderNumber = OrderNumberGenerator.Generate()
            };

            await _orderRepository.AddAsync(order);
            customer.Balance -= dto.PriceAzn;

            var balanceTransaction = new BalanceTransaction
            {
                CustomerId = customerId,
                Type = BalanceTransactionType.Debit,
                Amount = dto.PriceAzn,
                Description = "Sifariş yaradıldı",
                OrderId = order.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _balanceTransactionRepository.AddAsync(balanceTransaction);

            var notification = new Notification
            {
                CustomerId = customerId,
                Type = NotificationType.Info,
                Title = "Sifariş yaradıldı",
                Message = "Sifarişiniz yoxlanılır.",
                OrderId = order.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);

            await _unitOfWork.CommitAsync();

            return new("Order created successfully");
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<ResultDto<List<AdminOrderGetDto>>> GetAllAsync()
    {
        var orders = await _orderRepository
            .GetAll().AsNoTracking()
            .Include(x => x.Customer)
            .ThenInclude(x => x.AppUser)
            .Include(x => x.Country)
            .ToListAsync();

        var result = orders.Select(x => new AdminOrderGetDto
        {
            Id = x.Id,
            CustomerFirstName = x.Customer.AppUser.FirstName,
            CustomerLastName = x.Customer.AppUser.LastName,
            CustomerCode = x.Customer.CustomerCode,
            CountryName = x.Country.Name,
            StoreName = x.StoreName,
            StoreUrl = x.StoreUrl,
            Quantity = x.Quantity,
            PriceAzn = x.PriceAzn,
            Status = x.Status,
            ReviewedByUserId = x.ReviewedByUserId,
            ReviewedAt = x.ReviewedAt,
            AdminNote = x.AdminNote,
            CreatedAt = x.CreatedAt,
            OrderNumber = x.OrderNumber
        }).ToList();

        return new(result);
    }

    public async Task<ResultDto<UserOrderGetDto>> GetUserOrderAsync(Guid customerId, Guid orderId)
    {
        var order = await _orderRepository.GetAsync(x => x.Id == orderId && x.CustomerId == customerId, x => x.Country);
        if (order == null)
        {
            throw new NotFoundException("Order is not found");
        }

        var result = new UserOrderGetDto
        {
            Id = order.Id,
            CountryName = order.Country.Name,
            StoreName = order.StoreName,
            StoreUrl = order.StoreUrl,
            Quantity = order.Quantity,
            PriceAzn = order.PriceAzn,
            Status = order.Status,
            AdminNote = order.AdminNote,
            CreatedAt = order.CreatedAt,
            OrderNumber = order.OrderNumber
        };

        return new(result);
    }

    public async Task<ResultDto<List<UserOrderGetDto>>> GetUserOrdersAsync(Guid customerId)
    {
        var orders = await _orderRepository.GetAll().AsNoTracking().Include(x => x.Country).Where(x => x.CustomerId == customerId)
            .Select(x => new UserOrderGetDto
            {
                Id = x.Id,
                CountryName = x.Country.Name,
                StoreName = x.StoreName,
                StoreUrl = x.StoreUrl,
                Quantity = x.Quantity,
                PriceAzn = x.PriceAzn,
                Status = x.Status,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                OrderNumber = x.OrderNumber
            }).ToListAsync();

        return new(orders);
    }

    public async Task<ResultDto<PaginatedUserOrdersDto>> GetUserOrdersFilteredAsync(Guid customerId, Guid? countryId, OrderStatus? status, int pageNumber = 1, int pageSize = 5)
    {
        var query = _orderRepository.GetAll().AsNoTracking()
            .Include(x => x.Country)
            .Where(x => x.CustomerId == customerId);

        if (countryId.HasValue && countryId != Guid.Empty)
        {
            query = query.Where(x => x.CountryId == countryId);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status);
        }

        query = query.OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var orders = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UserOrderGetDto
            {
                Id = x.Id,
                CountryName = x.Country.Name,
                StoreName = x.StoreName,
                StoreUrl = x.StoreUrl,
                Quantity = x.Quantity,
                PriceAzn = x.PriceAzn,
                Status = x.Status,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                OrderNumber = x.OrderNumber
            }).ToListAsync();

        return new(new PaginatedUserOrdersDto
        {
            Orders = orders,
            TotalCount = totalCount,
            TotalPages = totalPages,
            CurrentPage = pageNumber,
            PageSize = pageSize
        });
    }

    public async Task<ResultDto> ReviewAsync(string adminUserId, AdminOrderReviewDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await _orderRepository.GetByIdAsync(dto.Id);
            if (order == null)
            {
                throw new NotFoundException("Order is not found");
            }

            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be reviewed");
            }

            order.Status = dto.Status;
            order.AdminNote = dto.AdminNote;
            order.ReviewedByUserId = adminUserId;
            order.ReviewedAt = DateTime.UtcNow;


            if (dto.Status == OrderStatus.Cancelled)
            {
                var customer = await _customerRepository.GetByIdAsync(order.CustomerId);

                customer.Balance += order.PriceAzn;

                await _balanceTransactionRepository.AddAsync(new BalanceTransaction
                {
                    CustomerId = order.CustomerId,
                    Type = BalanceTransactionType.Credit,
                    Amount = order.PriceAzn,
                    Description = "Sifariş ləğv edildi - məbləğ geri qaytarıldı",
                    OrderId = order.Id,
                    CreatedAt = DateTime.UtcNow
                });

                await _notificationRepository.AddAsync(new Notification
                {
                    CustomerId = order.CustomerId,
                    Type = NotificationType.Warning,
                    Title = "Sifariş ləğv edildi",
                    Message = "Məbləğ balansınıza qaytarıldı.",
                    OrderId = order.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else if (dto.Status == OrderStatus.Approved)
            {
                await _notificationRepository.AddAsync(new Notification
                {
                    CustomerId = order.CustomerId,
                    Type = NotificationType.Success,
                    Title = "Sifariş təsdiqləndi",
                    Message = "Sifarişiniz təsdiqləndi.Zəhmət olmasa bəyan edin!",
                    OrderId = order.Id,
                    CreatedAt = DateTime.UtcNow
                });

                await _packageRepository.AddAsync(new Package
                {
                    OrderId = order.Id,
                    DeclaredPrice = order.PriceAzn,
                    CustomerId = order.CustomerId,
                    CountryId = order.CountryId,
                    StoreName = order.StoreName,
                    Quantity = order.Quantity,
                    Status = PackageStatus.WaitingForDeclaration,
                    CreatedAt = DateTime.UtcNow,
                    TrackingNumber = TrackingNumberGenerator.Generate()
                });

                var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
                if (customer == null)
                {
                    throw new NotFoundException("Customer is not found");
                }
                var user = await _userManager.FindByIdAsync(customer.AppUserId);
                if (user != null)
                {
                    var emailBody = GenerateDeclarationReadyEmailBody(user.FirstName, user.LastName, order.OrderNumber);

                    await _emailService.SendEmailAsync(user.Email, "Bağlamanız bəyan üçün hazırdır", emailBody);
                }
            }

            _orderRepository.Update(order);

            await _unitOfWork.CommitAsync();

            return new ResultDto("Order reviewed successfully");
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

    }

    private string GenerateDeclarationReadyEmailBody(string firstName, string lastName, string orderNumber)
    {
        return $@"
<!DOCTYPE html>
<html lang='az'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>Bağlama Bəyan üçün Hazırdır</title>

<style>

body {{
font-family: 'Segoe UI', sans-serif;
background:#f4f6fb;
padding:20px;
}}

.container {{
max-width:600px;
margin:auto;
background:white;
border-radius:10px;
overflow:hidden;
box-shadow:0 10px 30px rgba(0,0,0,0.1);
}}

.header {{
background:#4f46e5;
color:white;
padding:30px;
text-align:center;
}}

.content {{
padding:30px;
font-size:16px;
line-height:1.7;
color:#333;
}}

.highlight {{
color:#4f46e5;
font-weight:600;
}}

.box {{
background:#f8f9ff;
border-left:4px solid #4f46e5;
padding:20px;
margin:25px 0;
border-radius:6px;
}}

.footer {{
text-align:center;
font-size:12px;
color:#888;
padding:20px;
border-top:1px solid #eee;
}}

</style>
</head>

<body>

<div class='container'>

<div class='header'>
<h1>📦 Bağlamanız bəyan üçün hazırdır!</h1>
</div>

<div class='content'>

<p>Hörmətli <span class='highlight'>{firstName} {lastName}</span>,</p>

<p>
Sizin <strong>{orderNumber}</strong> nömrəli bağlamanız 
<span class='highlight'>bəyan üçün hazırdır</span>.
</p>

<div class='box'>

<p>
Bağlamanız <strong>xarici anbarımıza daxil olduğu andan etibarən</strong> 
çatdırılma ödənişini edə bilməyiniz mümkündür.
</p>

<p>
Bağlamanızın yola düşməsi üçün <strong>bəyan etməyiniz mütləqdir</strong>.
</p>

<p>
Əks təqdirdə bağlamanızın ölkəmizə göndərişi mümkün olmayacaqdır.
</p>

</div>

</div>

<div class='footer'>
<p><strong>Limak Kargo</strong></p>
<p>Sürətli və güvənilir çatdırılma xidməti</p>
</div>

</div>

</body>
</html>";
    }

}
