using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.TariffDtos;
using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Application.Dtos.ShipmentTypeDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class TariffController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";

    public async Task<IActionResult> Index()
    {
        var tariffResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Tariffs/get-all-tariffs");

        if (!tariffResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var tariffList = await tariffResponse.Content.ReadFromJsonAsync<ResultDto<List<TariffGetDto>>>() ?? new();
        var model = tariffList.Data ?? new List<TariffGetDto>();

        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResponse = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Tariffs/delete-tariff/{id}");

        var deleteResult = await deleteResponse.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!deleteResult.IsSucceed)
        {
            return NotFound(deleteResult.Message);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadDropdownData();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TariffCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownData();
            return View(dto);
        }

        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Tariffs/create-tariff", dto);

        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            await LoadDropdownData();
            return View(dto);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var response = await _httpClient.GetAsync(
            $"{ApiBaseUrl}/Tariffs/get-update-dto/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var tariffResult = await response.Content.ReadFromJsonAsync<ResultDto<TariffUpdateDto>>() ?? new();

        await LoadDropdownData();
        return View(tariffResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TariffUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownData();
            return View(dto);
        }

        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Tariffs/update-tariff", dto);

        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            await LoadDropdownData();
            return View(dto);
        }

        return RedirectToAction("Index");
    }

    private async Task LoadDropdownData()
    {
        var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
        var countriesList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();
        ViewBag.Countries = countriesList.Data ?? new List<CountryGetDto>();

        var deliveryTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/DeliveryTypes/get-all-delivery-types");
        var deliveryTypesList = await deliveryTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<DeliveryTypeGetDto>>>() ?? new();
        ViewBag.DeliveryTypes = deliveryTypesList.Data ?? new List<DeliveryTypeGetDto>();

        var shipmentTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/ShipmentTypes/get-all-shipment-types");
        var shipmentTypesList = await shipmentTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<ShipmentTypeGetDto>>>() ?? new();
        ViewBag.ShipmentTypes = shipmentTypesList.Data ?? new List<ShipmentTypeGetDto>();
    }
}
