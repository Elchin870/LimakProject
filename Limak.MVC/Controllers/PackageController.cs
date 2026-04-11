using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.PackageDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.MVC.ViewModels.PackageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;
[Authorize(Roles = "Member")]
public class PackageController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/UserPackages/get-user-packages/{customerResult.Data!.Id}");
        var result = await response.Content.ReadFromJsonAsync<ResultDto<List<UserPackageListDto>>>() ?? new();

        var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
        var countriesResult = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();

        PackageViewModel model = new()
        {
            UserPackages = result.Data!,
            Countries = countriesResult.Data ?? new List<CountryGetDto>()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> PayForPackage(PackagePaymentDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customerResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customerResponse.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();


        dto.CustomerId = customerResult.Data!.Id;

        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/UserPackages/pay-package", dto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (result.IsSucceed)
        {
            TempData["SuccessMessage"] = result.Message;
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }
        return RedirectToAction("Index");
    }
}
