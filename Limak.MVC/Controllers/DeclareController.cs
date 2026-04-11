using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.PackageDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.MVC.ViewModels.DeclareViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;
[Authorize(Roles = "Member")]
public class DeclareController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/UserPackages/get-waiting-for-declare-packages/{customerResult.Data!.Id}");
        var result = await response.Content.ReadFromJsonAsync<ResultDto<List<UserPackageListDto>>>() ?? new();

        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            return BadRequest(result.Message);
        }

        DeclareViewModel model = new DeclareViewModel()
        {
            UserPackages = result.Data!
        };

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Declare(Guid id)
    {
        var model = await LoadDeclareFormData();
        
        ViewBag.PackageId = id;
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Declare(UserPackageDeclareDto declareDto)
    {
        if (!ModelState.IsValid)
        {
            var model = await LoadDeclareFormData();
            return View(model);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();
        declareDto.CustomerId = customerResult.Data!.Id;
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/UserPackages/declare-package", declareDto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            var model = await LoadDeclareFormData();
            return View(model);
        }
        return RedirectToAction("Index");
    }

    private async Task<DeclareViewModel> LoadDeclareFormData()
    {
        var model = new DeclareViewModel();

        var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
        var countriesList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<Limak.Application.Dtos.CountryDtos.CountryGetDto>>>() ?? new();
        model.Countries = countriesList.Data ?? new();

        var deliveryTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/DeliveryTypes/get-all-delivery-types");
        var deliveryTypesList = await deliveryTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<Limak.Application.Dtos.DeliveryTypeDtos.DeliveryTypeGetDto>>>() ?? new();
        model.DeliveryTypes = deliveryTypesList.Data ?? new();

        var shipmentTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/ShipmentTypes/get-all-shipment-types");
        var shipmentTypesList = await shipmentTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<Limak.Application.Dtos.ShipmentTypeDtos.ShipmentTypeGetDto>>>() ?? new();
        model.ShipmentTypes = shipmentTypesList.Data ?? new();

        var officesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Offices/get-all-offices");
        var officesList = await officesResponse.Content.ReadFromJsonAsync<ResultDto<List<Limak.Application.Dtos.OfficeDtos.OfficeGetDto>>>() ?? new();
        model.Offices = officesList.Data ?? new();

        var kargomatsResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Kargomats/get-all-kargomats");
        var kargomatsList = await kargomatsResponse.Content.ReadFromJsonAsync<ResultDto<List<Limak.Application.Dtos.KargomatDtos.KargomatGetDto>>>() ?? new();
        model.Kargomats = kargomatsList.Data ?? new();

        return model;
    }
}
