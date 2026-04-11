using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.OrderDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Domain.Enums;
using Limak.MVC.ViewModels.UserPanelViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;
[Authorize(Roles = "Member")]
public class UserPanelController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index(Guid? countryId, OrderStatus? status, int pageNumber = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();
        
        if (customerResult.Data == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var filterParams = new Dictionary<string, string>
        {
            { "customerId", customerResult.Data.Id.ToString() },
            { "pageNumber", pageNumber.ToString() },
            { "pageSize", "5" }
        };

        if (countryId.HasValue && countryId != Guid.Empty)
        {
            filterParams.Add("countryId", countryId.ToString());
        }

        if (status.HasValue)
        {
            filterParams.Add("status", ((int)status).ToString());
        }

        var queryString = string.Join("&", filterParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var ordersResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/UserOrders/filtered/{customerResult.Data.Id}?{queryString}");
        var ordersResult = await ordersResponse.Content.ReadFromJsonAsync<ResultDto<PaginatedUserOrdersDto>>() ?? new();

        var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
        var countries = new List<CountryFilterItem>();

        try
        {
            var countryResults = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>();
            if (countryResults?.Data != null)
            {
                countries = countryResults.Data.Select(c => new CountryFilterItem { Id = c.Id, Name = c.Name }).ToList();
            }
        }
        catch { }

        UserPanelViewModel model = new()
        {
            UserOrders = ordersResult.Data?.Orders ?? new List<UserOrderGetDto>(),
            Countries = countries,
            SelectedCountryId = countryId,
            SelectedStatus = status,
            CurrentPage = pageNumber,
            TotalPages = ordersResult.Data?.TotalPages ?? 1,
            TotalOrders = ordersResult.Data?.TotalCount ?? 0
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserOrderCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var orderResponse = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/UserOrders/create-order/{customerResult.Data!.Id}", dto);
        var orderResult = await orderResponse.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!orderResponse.IsSuccessStatusCode || !orderResult.IsSucceed)
        {
            ModelState.AddModelError("", orderResult.Message ?? "An error occurred while creating the order.");
            return View(dto);
        }

        return RedirectToAction("Index");
    }
}
