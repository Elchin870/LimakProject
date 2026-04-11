using Limak.Application.Dtos.CountryAddressDtos;
using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.MVC.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;
[Authorize(Roles = "Member")]
public class AddressController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var addressResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/CountryAddresses/get-all-addresses");
        var addressList = await addressResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryAddressGetDto>>>() ?? new();

        AddressViewModel model = new AddressViewModel()
        {
            Customer = customerResult.Data!,
            CountryAddress = addressList.Data ?? new List<CountryAddressGetDto>()
        };
        return View(model);

    }
}
