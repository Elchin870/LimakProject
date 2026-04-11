using CloudinaryDotNet.Actions;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CalcullatorDtos;
using Limak.Application.Dtos.PackageDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Application.Helpers;
using Limak.Domain.Entities;
using Limak.Domain.Enums;
using Limak.Persistence.Implementations.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class PackageService : IPackageService
{
    private readonly IPackageRepository _packageRepository;
    private readonly IShipmentCalculatorService _shipmentCalculatorService;
    private readonly ICustomerRepository _customerRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKargomatRepository _kargomatRepository;
    private readonly IBalanceTransactionRepository _balanceTransactionRepository;
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<AppUser> _appUserManager;
    public PackageService(IPackageRepository packageRepository, IShipmentCalculatorService shipmentCalculatorService, ICustomerRepository customerRepository, INotificationRepository notificationRepository, IUnitOfWork unitOfWork, IKargomatRepository kargomatRepository, IBalanceTransactionRepository balanceTransactionRepository, IEmailService emailService, UserManager<AppUser> userManager, IOrderRepository orderRepository, UserManager<AppUser> appUserManager)
    {
        _packageRepository = packageRepository;
        _shipmentCalculatorService = shipmentCalculatorService;
        _customerRepository = customerRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _kargomatRepository = kargomatRepository;
        _balanceTransactionRepository = balanceTransactionRepository;
        _emailService = emailService;
        _userManager = userManager;
        _orderRepository = orderRepository;
        _appUserManager = appUserManager;
    }

    public async Task<ResultDto> CreatePackageAdmin(string adminUserId, AdminCreatePackageDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var customer = await _customerRepository.GetAll().FirstOrDefaultAsync(x => x.CustomerCode == dto.CustomerCode);

            if (customer is null)
            {
                throw new NotFoundException("Customer is not found");
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                CountryId = dto.CountryId,
                StoreName = dto.StoreName,
                Status = OrderStatus.Approved,
                Quantity = dto.Quantity,
                PriceAzn = dto.Price,
                OrderNumber = OrderNumberGenerator.Generate(),
                CreatedAt = DateTime.UtcNow,
                StoreUrl = string.Empty,
                ReviewedByUserId = adminUserId,
                ReviewedAt = DateTime.UtcNow
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            var package = new Package
            {
                CustomerId = customer.Id,
                CountryId = dto.CountryId,
                OrderId = order.Id,
                StoreName = dto.StoreName,
                Status = PackageStatus.WaitingForDeclaration,
                Quantity = dto.Quantity,
                DeclaredPrice = dto.Price,
                TrackingNumber = TrackingNumberGenerator.Generate(),
                CreatedAt = DateTime.UtcNow
            };



            await _packageRepository.AddAsync(package);

            await _notificationRepository.AddAsync(new Notification
            {
                CustomerId = customer.Id,
                Type = NotificationType.Info,
                Title = "Bağlama bəyan üçün hazırdır",
                Message = "Bağlama bəyan üçün hazırdır. Zəhmət olmasa bəyan edin!",
                CreatedAt = DateTime.UtcNow,
            });

            var user = await _appUserManager.FindByIdAsync(customer.AppUserId);
            if (user != null)
            {
                var emailBody = GenerateDeclarationReadyEmailBody(user.FirstName, user.LastName, order.OrderNumber);
                await _emailService.SendEmailAsync(user.Email, "Bağlama bəyan üçün hazırdır", emailBody);
            }

            await _unitOfWork.CommitAsync();

            return new ResultDto("Package created successfully");
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<ResultDto> DeclarePackage(UserPackageDeclareDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer is null)
        {
            throw new NotFoundException("Customer is not found");
        }

        var package = await _packageRepository.GetByIdAsync(dto.PackageId);
        if (package is null)
        {
            throw new NotFoundException("Package is not found");
        }

        if (package.Status != PackageStatus.WaitingForDeclaration && package.Status != PackageStatus.NeedCorrection)
        {
            throw new InvalidOperationException("Package cannot be declared");
        }

        if (package.CustomerId != dto.CustomerId)
        {
            throw new UnauthorizedAccessException();
        }

        package.DeliveryTypeId = dto.DeliveryTypeId;
        package.ShipmentTypeId = dto.ShipmentTypeId;
        package.KargomatId = dto.KargomatId;
        package.Address = dto.Address;
        package.OfficeId = dto.OfficeId;
        package.Status = PackageStatus.Declared;

        _packageRepository.Update(package);
        await _packageRepository.SaveChangesAsync();

        return new("Package declared successfully");

    }

    public async Task<ResultDto<List<AdminPackageListDto>>> GetAdminPackages()
    {
        var result = await _packageRepository.GetAll().AsNoTracking()
          .Include(x => x.Customer)
          .ThenInclude(x => x.AppUser)
          .Include(x => x.Country)
          .Include(x => x.Order)
          .Include(x => x.Kargomat)
          .Include(x => x.ShipmentType)
          .Include(x => x.DeliveryType)
          .Include(x => x.Office)
          .Where(x => x.Status != PackageStatus.WaitingForDeclaration && x.Status != PackageStatus.NeedCorrection && x.Status != PackageStatus.Declared)
          .Select(x => new AdminPackageListDto
          {
              Id = x.Id,
              CountryName = x.Country.Name,
              OrderNumber = x.Order != null ? x.Order.OrderNumber : string.Empty,
              TrackingNumber = x.TrackingNumber,
              CustomerCode = x.Customer.CustomerCode,
              CustomerFirstName = x.Customer.AppUser.FirstName,
              CustomerLastName = x.Customer.AppUser.LastName,
              StoreName = x.StoreName,
              ShippingPrice = x.ShippingPrice,
              Status = x.Status,
              Address = x.Address,
              CreatedAt = x.CreatedAt,
              KargomatAddress = x.Kargomat != null ? x.Kargomat.FullAddress : string.Empty,
              ShipmentType = x.ShipmentType != null ? x.ShipmentType.Name : string.Empty,
              DeliveryType = x.DeliveryType != null ? x.DeliveryType.Name : string.Empty,
              Weight = x.Weight,
              OfficeCity = x.Office != null ? x.Office.City : string.Empty,
              OfficeMetroStation = x.Office != null ? x.Office.MetroStation : string.Empty,
              IsPaid = x.IsPaid,
              PaidAt = x.PaidAt

          }).ToListAsync();

        return new(result);
    }

    public async Task<ResultDto<List<AdminPackageListDto>>> GetDeclaredPackagesForAdmin()
    {
        var result = await _packageRepository.GetAll().Where(x => x.Status == PackageStatus.Declared).AsNoTracking()
          .Include(x => x.Customer)
          .ThenInclude(x => x.AppUser)
          .Include(x => x.Country)
          .Include(x => x.Order)
          .Include(x => x.Kargomat)
          .Include(x => x.ShipmentType)
          .Include(x => x.DeliveryType)
          .Select(x => new AdminPackageListDto
          {
              Id = x.Id,
              CountryName = x.Country.Name,
              OrderNumber = x.Order != null ? x.Order.OrderNumber : string.Empty,
              TrackingNumber = x.TrackingNumber,
              CustomerCode = x.Customer.CustomerCode,
              CustomerFirstName = x.Customer.AppUser.FirstName,
              CustomerLastName = x.Customer.AppUser.LastName,
              StoreName = x.StoreName,
              ShippingPrice = x.ShippingPrice,
              Status = x.Status,
              Address = x.Address,
              CreatedAt = x.CreatedAt,
              KargomatAddress = x.Kargomat != null ? x.Kargomat.ShortAddress : string.Empty,
              ShipmentType = x.ShipmentType != null ? x.ShipmentType.Name : string.Empty,
              DeliveryType = x.DeliveryType != null ? x.DeliveryType.Name : string.Empty,
              Weight = x.Weight,
          }).ToListAsync();

        return new(result);
    }

    public async Task<ResultDto<List<UserPackageListDto>>> GetUserPackages(Guid customerId)
    {
        var packages = await _packageRepository.GetAll().Where(x => x.CustomerId == customerId && x.Status != PackageStatus.WaitingForDeclaration && x.Status != PackageStatus.NeedCorrection && x.Status != PackageStatus.Declared).AsNoTracking()
            .Include(x => x.Order)
            .Include(x => x.Country)
            .Select(x => new UserPackageListDto
            {
                Id = x.Id,
                OrderNumber = x.Order != null ? x.Order.OrderNumber : string.Empty,
                TrackingNumber = x.TrackingNumber,
                StoreName = x.StoreName,
                ShippingPrice = x.ShippingPrice,
                Status = x.Status,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                OrderPrice = x.Order != null ? x.Order.PriceAzn : 0,
                Weight = x.Weight,
                CountryName = x.Country.Name,
                IsPaid = x.IsPaid
            }).ToListAsync();

        return new(packages);
    }

    public async Task<ResultDto<List<UserPackageListDto>>> GetWaitingForDeclarePackages(Guid customerId)
    {
        var packages = await _packageRepository.GetAll().Where(x => x.CustomerId == customerId && (x.Status == PackageStatus.WaitingForDeclaration || x.Status == PackageStatus.NeedCorrection)).AsNoTracking()
            .Include(x => x.Order)
            .Include(x => x.Country)
            .Select(x => new UserPackageListDto
            {
                Id = x.Id,
                OrderNumber = x.Order != null ? x.Order.OrderNumber : string.Empty,
                TrackingNumber = x.TrackingNumber,
                StoreName = x.StoreName,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                OrderPrice = x.Order != null ? x.Order.PriceAzn : 0,
                CountryName = x.Country.Name,
                IsPaid = x.IsPaid
            }).ToListAsync();
        return new(packages);
    }

    public async Task<ResultDto> PayForPackage(PackagePaymentDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var package = await _packageRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.PackageId && x.CustomerId == dto.CustomerId);
            if (package is null)
            {
                throw new NotFoundException("Package is not found");
            }

            if (package.Status == PackageStatus.WaitingForDeclaration || package.Status == PackageStatus.NeedCorrection)
            {
                throw new InvalidOperationException("Package is not declared yet");
            }

            if (package.ShippingPrice is null)
            {
                throw new InvalidOperationException("Shipping price is not calculated yet");
            }

            if (package.IsPaid)
            {
                throw new InvalidOperationException("Package is already paid");
            }

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer is null)
            {
                throw new NotFoundException("Customer is not found");
            }

            if (customer.Balance < package.ShippingPrice)
            {
                throw new InvalidOperationException("Balansınız kifayət etmir");
            }

            customer.Balance -= package.ShippingPrice.Value;
            package.IsPaid = true;
            package.PaidAt = DateTime.UtcNow;

            var balanceTransaction = new BalanceTransaction
            {
                CustomerId = dto.CustomerId,
                Type = BalanceTransactionType.Debit,
                Amount = package.ShippingPrice.Value,
                Description = "Bağlamanın ödənişi edildi",
                OrderId = package.OrderId,
                CreatedAt = DateTime.UtcNow
            };

            await _balanceTransactionRepository.AddAsync(balanceTransaction);

            var notification = new Notification
            {
                CustomerId = dto.CustomerId,
                Type = NotificationType.Success,
                Title = "Ödəniş olundu",
                Message = "Bağlamanın ödənişi uğurla həyata keçdi.",
                OrderId = package.OrderId,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);

            _customerRepository.Update(customer);
            _packageRepository.Update(package);
            await _unitOfWork.CommitAsync();

            return new("Payment successful");
        }

        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<ResultDto> UpdatePackageStatus(AdminPackageUpdateStatusDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var package = await _packageRepository.GetAll().Include(x => x.Customer).ThenInclude(x => x.AppUser).Include(x => x.Order).Include(x => x.Office).Where(x => x.Id == dto.PackageId).FirstOrDefaultAsync();

            if (package is null)
                throw new NotFoundException("Package not found");

            var currentStatus = package.Status;
            var newStatus = dto.Status;

            if (!PackageStatusTransitions.AllowedTransitions.TryGetValue(currentStatus, out var allowedStatuses) || !allowedStatuses.Contains(newStatus))
            {
                throw new InvalidOperationException($"Invalid status transition from {currentStatus} to {newStatus}");
            }

            package.AdminNote = dto.AdminNote;
            var user = await _userManager.FindByIdAsync(package.Customer.AppUserId);
            switch (newStatus)
            {
                case PackageStatus.NeedCorrection:
                    await AddNotification(package.CustomerId, "Bəyan düzəliş tələb edir", "Bəyannamədə səhv var. Zəhmət olmasa yenidən bəyan edin.", NotificationType.Warning, package.OrderId.Value);
                    break;

                case PackageStatus.ReadyForWarehouse:

                    package.Weight = dto.Weight;
                    package.Width = dto.Width;
                    package.Length = dto.Length;
                    package.Height = dto.Height;

                    if (package.DeliveryTypeId is null || package.ShipmentTypeId is null)
                        throw new InvalidOperationException("DeliveryType or ShipmentType missing");
                    var calculateDto = new ShipmentCalculateDto
                    {
                        CountryId = package.CountryId,
                        DeliveryTypeId = package.DeliveryTypeId.Value,
                        ShipmentTypeId = package.ShipmentTypeId.Value,
                        Weight = dto.Weight,
                        Width = dto.Width,
                        Length = dto.Length,
                        Height = dto.Height
                    };

                    var shippingPrice = await _shipmentCalculatorService.CalculatePriceAsync(calculateDto);

                    var totalPrice = shippingPrice.Data;

                    if (package.KargomatId.HasValue)
                    {
                        var kargomat = await _kargomatRepository.GetByIdAsync(package.KargomatId.Value);

                        if (kargomat == null)
                        {
                            throw new NotFoundException("Kargomat not found");
                        }

                        totalPrice += kargomat.Price;
                    }

                    package.ShippingPrice = totalPrice;

                    await AddNotification(package.CustomerId, "Çatdırılma haqqı hesablandı", "Bağlamanın çatdırılma haqqı hesablandı. Çatdırılma üçün hazırlanır", NotificationType.Info, package.OrderId);

                    if (user != null)
                    {
                        var emailBody = GenerateReadyForWarehouseEmailBody(user.FirstName, package.ShippingPrice.Value, user.LastName);
                        await _emailService.SendEmailAsync(user.Email, "Bağlama Bəyanlaması Tamamlandı", emailBody);
                    }

                    break;

                case PackageStatus.InWarehouse:

                    await AddNotification(package.CustomerId, "Bağlama anbara daxil oldu", "Bağlamanız xarici anbardadır.", NotificationType.Info, package.OrderId);

                    break;

                case PackageStatus.InTransit:

                    await AddNotification(package.CustomerId, "Bağlama yola düşdü", "Bağlamanız Azərbaycana göndərildi.", NotificationType.Info, package.OrderId);

                    break;

                case PackageStatus.ArrivedToCountry:

                    await AddNotification(package.CustomerId, "Bağlama ölkəyə çatdı", "Bağlamanız Azərbaycana çatdı.Gömrük yoxlanışındadır", NotificationType.Info, package.OrderId);

                    break;

                case PackageStatus.ReadyForDelivery:

                    await AddNotification(package.CustomerId, "Bağlama təhvilə hazırdır", "Bağlamanızı təhvil ala bilərsiniz.", NotificationType.Info, package.OrderId);

                    if (user != null && package.ShipmentTypeId != null)
                    {
                        if (package.KargomatId.HasValue)
                        {
                            var kargomat = await _kargomatRepository.GetByIdAsync(package.KargomatId.Value);

                            var emailBody = GenerateKargomatReadyEmailBody(user.FirstName, user.LastName, package.Order?.OrderNumber, kargomat.FullAddress, package.TrackingNumber);

                            await _emailService.SendEmailAsync(user.Email, "Bağlamanız kargomata çatdırıldı", emailBody);
                        }
                        else if (package.OfficeId.HasValue)
                        {
                            var city = package.Office.City;
                            var metro = package.Office.MetroStation;

                            string locationText = !string.IsNullOrWhiteSpace(metro)
                                ? $"Limak {city}-{metro}"
                                : $"Limak {city}";

                            var emailBody = GenerateReadyForDeliveryEmailBody(user.FirstName, user.LastName, package.Order?.OrderNumber, package.TrackingNumber, locationText);

                            await _emailService.SendEmailAsync(user.Email, "Bağlamanız təhvil məntəqəsindədir", emailBody);
                        }
                    }

                    break;

                case PackageStatus.Delivered:

                    await AddNotification(package.CustomerId, "Bağlama təhvil verildi", "Bağlamanız uğurla təhvil verildi.", NotificationType.Success, package.OrderId);

                    break;
            }

            package.Status = newStatus;

            _packageRepository.Update(package);

            await _unitOfWork.CommitAsync();

            return new ResultDto("Package status updated successfully");
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task AddNotification(Guid customerId, string title, string message, NotificationType type, Guid? orderId)
    {
        await _notificationRepository.AddAsync(new Notification
        {
            CustomerId = customerId,
            Title = title,
            Message = message,
            Type = type,
            OrderId = orderId,
            CreatedAt = DateTime.UtcNow
        });
    }

    private string GenerateReadyForWarehouseEmailBody(string firstName, decimal shippingPrice, string lastName)
    {
        return $@"
<!DOCTYPE html>
<html lang='az' dir='ltr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Bağlama Bəyanlaması Tamamlandı</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 20px;
            min-height: 100vh;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: white;
            border-radius: 12px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 40px 20px;
            text-align: center;
        }}
        .header h1 {{
            font-size: 28px;
            margin-bottom: 10px;
            font-weight: 700;
        }}
        .header p {{
            font-size: 14px;
            opacity: 0.9;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .greeting {{
            font-size: 18px;
            color: #333;
            margin-bottom: 20px;
            font-weight: 600;
        }}
        .message {{
            font-size: 16px;
            color: #555;
            line-height: 1.8;
            margin-bottom: 30px;
        }}
        .highlight {{
            color: #667eea;
            font-weight: 600;
        }}
        .price-section {{
            background: #f8f9fa;
            border-left: 4px solid #667eea;
            padding: 20px;
            margin: 30px 0;
            border-radius: 8px;
        }}
        .price-label {{
            font-size: 14px;
            color: #666;
            margin-bottom: 10px;
            font-weight: 500;
        }}
        .price-value {{
            font-size: 32px;
            color: #667eea;
            font-weight: 700;
            display: flex;
            align-items: center;
        }}
        .currency {{
            font-size: 20px;
            margin-left: 10px;
            color: #999;
        }}
        .payment-options {{
            background: #f0f4ff;
            padding: 20px;
            border-radius: 8px;
            margin: 25px 0;
        }}
        .payment-options h3 {{
            color: #333;
            font-size: 16px;
            margin-bottom: 15px;
            font-weight: 600;
        }}
        .option {{
            display: flex;
            align-items: center;
            margin-bottom: 12px;
            font-size: 15px;
            color: #555;
        }}
        .option-icon {{
            width: 24px;
            height: 24px;
            background: #667eea;
            color: white;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-right: 12px;
            font-weight: bold;
            font-size: 14px;
        }}
        .footer {{
            background: #f8f9fa;
            padding: 30px;
            text-align: center;
            border-top: 1px solid #e0e0e0;
        }}
        .footer p {{
            font-size: 13px;
            color: #888;
            margin-bottom: 10px;
        }}
        .social-links {{
            margin-top: 15px;
        }}
        .social-links a {{
            display: inline-block;
            width: 40px;
            height: 40px;
            background: #667eea;
            color: white;
            border-radius: 50%;
            line-height: 40px;
            margin: 0 5px;
            text-decoration: none;
            font-size: 18px;
        }}
        .social-links a:hover {{
            background: #764ba2;
        }}
        .divider {{
            height: 1px;
            background: #e0e0e0;
            margin: 30px 0;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>✓ Bağlama Bəyanlaması Tamamlandı</h1>
            <p>Çatdırılma haqqı hesablandı</p>
        </div>
        <div class='content'>
            <div class='greeting'>Hörmətli {firstName} {lastName},</div>
            <div class='message'>
                Sizin bağlamanız uğurla şəkildə <span class='highlight'>bəyan olundu</span>. 
                Çatdırılma haqqı aşağıda göstərilmişdir.
            </div>
            
            <div class='price-section'>
                <div class='price-label'>Çatdırılma Haqqı</div>
                <div class='price-value'>
                    {shippingPrice:F2}
                    <span class='currency'>AZN</span>
                </div>
            </div>

            <div class='divider'></div>

            <div class='payment-options'>
                <h3>Ödəniş Seçimləri</h3>
                <div class='option'>
                    <div>Çatdırılma haqqını <strong>onlayn</strong> ödəyin</div>
                </div>
                <div class='option'>
                    <div>Bağlamanı <strong>təhvil alarkən</strong> ödəyin</div>
                </div>
            </div>

            <div class='message'>
                Bağlamanız hazırlanır və tez bir zamanda yolda olacaqdır. 
                Hesabınızdan hər zaman bağlamanızın statusunu izləyə bilərsiniz.
                Qeyd:Çatdırılma növü kargomat olan bağlamaların ödənişini sadəcə onlayn edə bilərsiniz.Ödəniş etmədiyiniz təqdirdə bağlamanın çatdırılması mümkün olmayacaqdır.
            </div>
        </div>
        <div class='footer'>
            <p><strong>Limak Kargo</strong></p>
            <p>Sürətli və güvənilir çatdırılma xidməti</p>
            <p style='margin-top: 20px; font-size: 12px;'>
                © 2024 Limak Kargo. Bütün hüquqlar qorunur.
            </p>
        </div>
    </div>
</body>
</html>";
    }

    private string GenerateReadyForDeliveryEmailBody(string firstName, string lastName, string orderNumber, string trackingNumber, string location)
    {
        return $@"
<!DOCTYPE html>
<html lang='az'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>Bağlama Təhvil Məntəqəsindədir</title>

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
text-align:center;
padding:30px;
}}

.header h1 {{
font-size:24px;
}}

.content {{
padding:30px;
color:#333;
line-height:1.7;
font-size:16px;
}}

.highlight {{
color:#4f46e5;
font-weight:600;
}}

.box {{
background:#f8f9ff;
border-left:4px solid #4f46e5;
padding:20px;
margin:20px 0;
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
<h1>📦 Bağlamanız təhvil məntəqəsindədir!</h1>
</div>

<div class='content'>

<p>👤 Hörmətli <span class='highlight'>{firstName} {lastName}</span>,</p>

<p>
Sizin <span class='highlight'>{orderNumber}</span> nömrəli bağlamanız 
<span class='highlight'>{location}</span> filialı təhvil məntəqəsinə çatdırıldı.
</p>

<div class='box'>

<p>
📱 Bağlama nömrənizi və <strong>tracking kodu</strong> məntəqə əməkdaşına təqdim edərək,
bağlamanızı təhlükəsiz qaydada təhvil ala bilərsiniz.
</p>

<p>
Tracking kodu: <strong>{trackingNumber}</strong>
</p>

</div>

<p>
Bağlamanızı tez bir zamanda təhvil almağınızı tövsiyə edirik.
</p>

</div>

<div class='footer'>
<p><strong>Limak Kargo</strong></p>
<p>Sürətli və güvənilir çatdırılma xidməti</p>
</div>

</div>

</body>
</html>
";
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

    private string GenerateKargomatReadyEmailBody(string firstName, string lastName, string orderNumber, string kargomatAddress, string trackingNumber)
    {
        return $@"
<!DOCTYPE html>
<html lang='az'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>Bağlama Kargomata Çatdırıldı</title>

<style>

body {{
font-family:'Segoe UI',sans-serif;
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
background:#10b981;
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
color:#10b981;
font-weight:600;
}}

.box {{
background:#f0fdf4;
border-left:4px solid #10b981;
padding:20px;
margin:25px 0;
border-radius:6px;
}}

.code {{
font-size:24px;
font-weight:700;
letter-spacing:3px;
color:#10b981;
margin-top:10px;
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
<h1>📦 Bağlamanız kargomata çatdırıldı!</h1>
</div>

<div class='content'>

<p>👤 Hörmətli <span class='highlight'>{firstName} {lastName}</span>,</p>

<p>
Sizin <strong>{orderNumber}</strong> nömrəli bağlamanız 
<span class='highlight'>{kargomatAddress}</span> kargomatına çatdırıldı.
</p>

<div class='box'>

<p>
📦 Bağlamanızı kargomatdan götürmək üçün aşağıdakı şifrəni istifadə edin:
</p>

<div class='code'>
{trackingNumber}
</div>

<p>
Kargomat ekranına bu şifrəni daxil edərək bağlamanızı təhvil ala bilərsiniz.
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
