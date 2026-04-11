using Limak.Application.Dtos.BalanceTransactionDtos;
using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.NotificationDtos;
using Limak.Application.Dtos.PaymentDtos;
using Limak.Application.Dtos.PurchaseDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Domain.Enums;
using Limak.MVC.ViewModels.BalanceViewModels;
using Limak.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;
[Authorize(Roles = "Member")]
public class BalanceController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const int PAGE_SIZE = 10;
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index(int page = 1, int filterType = 0)
    {
        BalanceViewModel model = await SendCustomerInfo(page, filterType);
        return View(model);
    }

    private async Task<BalanceViewModel> SendCustomerInfo(int page = 1, int filterType = 0)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var payment = await _httpClient.GetAsync($"{ApiBaseUrl}/Payments/get-payment-by-appuser-id/{userId}");
        var paymentResult = await payment.Content.ReadFromJsonAsync<ResultDto<PaymentGetDto>>() ?? new();

        var balanceTransactions = await _httpClient.GetAsync($"{ApiBaseUrl}/BalanceTransactions/get-all-user/{customerResult.Data!.Id}");
        var balanceTransactionsResult = await balanceTransactions.Content.ReadFromJsonAsync<ResultDto<List<BalanceTransactionGetDto>>>() ?? new();

        var filteredTransactions = balanceTransactionsResult.Data ?? new List<BalanceTransactionGetDto>();
        
        if (filterType == 1) 
        {
            filteredTransactions = filteredTransactions.Where(t => t.Type == BalanceTransactionType.Debit).ToList();
        }
        else if (filterType == 2) 
        {
            filteredTransactions = filteredTransactions.Where(t => t.Type == BalanceTransactionType.Credit).ToList();
        }

        filteredTransactions = filteredTransactions.OrderByDescending(t => t.CreatedAt).ToList();

        int totalItems = filteredTransactions.Count;
        int skip = (page - 1) * PAGE_SIZE;
        var paginatedTransactions = filteredTransactions.Skip(skip).Take(PAGE_SIZE).ToList();

        BalanceViewModel model = new BalanceViewModel()
        {
            Customer = customerResult.Data!,
            Payment = paymentResult.Data ?? new PaymentGetDto(),
            BalanceTransactions = paginatedTransactions,
            CurrentPage = page,
            PageSize = PAGE_SIZE,
            TotalItems = totalItems,
            FilterType = filterType
        };
        return model;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(decimal amount)
    {
        await SendCustomerInfo();
        if (amount <= 0)
        {
            ModelState.AddModelError("", "Amount must be greater than 0!");
            return View("Index");
        }



        var purchase = new PurchaseCreateDto
        {
            Amount = amount,
            Description = "Medaxil",
            Currency = "AZN",
            RedirectUrl = "https://localhost:7001/Balance/RedirectPayment"
        };

        var purchaseResponse = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Payments/create-purchase", purchase);

        var purchaseResult = await purchaseResponse.Content.ReadFromJsonAsync<ResultDto<PurchaseGetDto>>() ?? new();

        if (!purchaseResult.IsSucceed)
        {
            ModelState.AddModelError("", purchaseResult.Message);
            return View("Index");
        }

        string paymentGatewayUrl = $"{purchaseResult.Data.Order.HppUrl}?id={purchaseResult.Data.Order.Id}&password={purchaseResult.Data.Order.Password}";

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        PaymentCreateDto payment = new PaymentCreateDto
        {
            AppUserId = userId!,
            Amount = amount,
            PurchaseId = purchaseResult.Data.Order.Id,
            Password = purchaseResult.Data.Order.Password,
            PaymentStatus = PaymentStatuses.Pending,
            CreatedDate = DateTime.UtcNow,
            Secret = purchaseResult.Data.Order.Secret,
        };

        var paymentResponse = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Payments/create-payment", payment);

        var paymentResult = await paymentResponse.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!paymentResult.IsSucceed)
        {
            ModelState.AddModelError("", paymentResult.Message);
            return View("Index");
        }

        return Redirect(paymentGatewayUrl);
    }


    public async Task<IActionResult> RedirectPayment(int id)
    {
        await SendCustomerInfo();
        var purchaseInfo = await _httpClient.GetAsync($"{ApiBaseUrl}/Payments/get-purchase-info/{id}");

        var purchaseInfoResult = await purchaseInfo.Content.ReadFromJsonAsync<ResultDto<PurchaseDetailDto>>() ?? new();
        if (!purchaseInfoResult.IsSucceed || !purchaseInfo.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", purchaseInfoResult.Message);
            return View("Index");
        }

        var payment = await _httpClient.GetAsync($"{ApiBaseUrl}/Payments/get-payment/{id}");
        var paymentResult = await payment.Content.ReadFromJsonAsync<ResultDto<PaymentGetDto>>() ?? new();

        if (!paymentResult.IsSucceed || !payment.IsSuccessStatusCode)
        {
            return NotFound(paymentResult.Message);
        }

        PaymentUpdateDto paymentUpdateDto = new PaymentUpdateDto()
        {
            Id = paymentResult.Data.Id,
            Amount = paymentResult.Data.Amount,
            AppUserId = paymentResult.Data.AppUserId,
            PurchaseId = paymentResult.Data.PurchaseId,
            Secret = paymentResult.Data.Secret,
            CreatedDate = paymentResult.Data.CreatedDate,
            Password = paymentResult.Data.Password,
            PaymentStatus = purchaseInfoResult.Data!.Order.Status
        };

        var updatePayment = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Payments/update-payment", paymentUpdateDto);

        var updatePaymentResult = await updatePayment.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!updatePaymentResult.IsSucceed || !updatePayment.IsSuccessStatusCode)
        {
            PaymentUpdateDto paymentError = new PaymentUpdateDto()
            {
                Id = paymentResult.Data.Id,
                Amount = paymentResult.Data.Amount,
                AppUserId = paymentResult.Data.AppUserId,
                PurchaseId = paymentResult.Data.PurchaseId,
                Secret = paymentResult.Data.Secret,
                CreatedDate = paymentResult.Data.CreatedDate,
                Password = paymentResult.Data.Password,
                PaymentStatus = PaymentStatuses.Pending
            };

            var errorPayment = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Payments/update-payment", paymentError);

            var errorPaymentResult = await errorPayment.Content.ReadFromJsonAsync<ResultDto>() ?? new();

            if (!errorPaymentResult.IsSucceed || !errorPayment.IsSuccessStatusCode)
            {
                return BadRequest(errorPaymentResult.Message);
            }
            return BadRequest(updatePaymentResult.Message);
        }

        if (purchaseInfoResult.Data.Order.Status == PaymentStatuses.Fullypaid && paymentResult.Data.PaymentStatus != PaymentStatuses.Fullypaid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
            var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();
            if (!customerResult.IsSucceed || !customer.IsSuccessStatusCode)
            {
                PaymentUpdateDto paymentError = new PaymentUpdateDto()
                {
                    Id = paymentResult.Data.Id,
                    Amount = paymentResult.Data.Amount,
                    AppUserId = paymentResult.Data.AppUserId,
                    PurchaseId = paymentResult.Data.PurchaseId,
                    Secret = paymentResult.Data.Secret,
                    CreatedDate = paymentResult.Data.CreatedDate,
                    Password = paymentResult.Data.Password,
                    PaymentStatus = PaymentStatuses.Pending
                };

                var errorPayment = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Payments/update-payment", paymentError);

                var errorPaymentResult = await errorPayment.Content.ReadFromJsonAsync<ResultDto>() ?? new();

                if (!errorPaymentResult.IsSucceed || !errorPayment.IsSuccessStatusCode)
                {
                    return BadRequest(errorPaymentResult.Message);
                }
                return NotFound(customerResult.Message);
            }

            IncreaseBalanceDto increaseBalanceDto = new IncreaseBalanceDto
            {
                CustomerId = customerResult.Data.Id,
                Amount = purchaseInfoResult.Data.Order.Amount
            };

            var balanceResponse = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Customers/increase-balance", increaseBalanceDto);

            var balanceResult = await balanceResponse.Content.ReadFromJsonAsync<ResultDto>();

            if (!balanceResponse.IsSuccessStatusCode || !balanceResult.IsSucceed)
            {
                return BadRequest(balanceResult.Message);
            }

            NotificationCreateDto notification = new NotificationCreateDto
            {
                CustomerId = customerResult.Data.Id,
                Message = $"{purchaseInfoResult.Data.Order.Amount} AZN hesabınıza medaxil oldu",
                Title = "Hesaba Medaxil",
                Type = NotificationType.Success,
                CreatedAt = DateTime.UtcNow
            };

            var notificationResponse = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Notifications/create-notification", notification);
            var notificationResult = await notificationResponse.Content.ReadFromJsonAsync<ResultDto>();

            if (!notificationResponse.IsSuccessStatusCode || !notificationResult.IsSucceed)
            {
                PaymentUpdateDto paymentError = new PaymentUpdateDto()
                {
                    Id = paymentResult.Data.Id,
                    Amount = paymentResult.Data.Amount,
                    AppUserId = paymentResult.Data.AppUserId,
                    PurchaseId = paymentResult.Data.PurchaseId,
                    Secret = paymentResult.Data.Secret,
                    CreatedDate = paymentResult.Data.CreatedDate,
                    Password = paymentResult.Data.Password,
                    PaymentStatus = PaymentStatuses.Pending
                };

                var errorPayment = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Payments/update-payment", paymentError);

                var errorPaymentResult = await errorPayment.Content.ReadFromJsonAsync<ResultDto>() ?? new();

                if (!errorPaymentResult.IsSucceed || !errorPayment.IsSuccessStatusCode)
                {
                    return BadRequest(errorPaymentResult.Message);
                }
                return NotFound(customerResult.Message);
            }

            var balanceTransaction = new BalanceTransactionCreateDto
            {
                Amount = purchaseInfoResult.Data.Order.Amount,
                CustomerId = customerResult.Data.Id,
                Description = "Hesaba Medaxil",
                Type = BalanceTransactionType.Credit,
                CreatedAt = DateTime.UtcNow,
            };

            var balanceTransactionResponse = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/BalanceTransactions/create-balance-transaction", balanceTransaction);
            var balanceTransactionResult = await balanceTransactionResponse.Content.ReadFromJsonAsync<ResultDto>();

            if (!balanceTransactionResponse.IsSuccessStatusCode || !balanceTransactionResult.IsSucceed)
            {
                PaymentUpdateDto paymentError = new PaymentUpdateDto()
                {
                    Id = paymentResult.Data.Id,
                    Amount = paymentResult.Data.Amount,
                    AppUserId = paymentResult.Data.AppUserId,
                    PurchaseId = paymentResult.Data.PurchaseId,
                    Secret = paymentResult.Data.Secret,
                    CreatedDate = paymentResult.Data.CreatedDate,
                    Password = paymentResult.Data.Password,
                    PaymentStatus = PaymentStatuses.Pending
                };
                var errorPayment = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Payments/update-payment", paymentError);
                var errorPaymentResult = await errorPayment.Content.ReadFromJsonAsync<ResultDto>() ?? new();
                if (!errorPaymentResult.IsSucceed || !errorPayment.IsSuccessStatusCode)
                {
                    return BadRequest(errorPaymentResult.Message);
                }
                return NotFound(customerResult.Message);
            }
        }

        return RedirectToAction("Index");
    }
}
