using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShipmentTypeDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ShipmentTypeController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient
            .GetAsync($"{ApiBaseUrl}/ShipmentTypes/get-all-shipment-types");

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        var list = await response.Content
            .ReadFromJsonAsync<ResultDto<List<ShipmentTypeGetDto>>>() ?? new();

        return View(list.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ShipmentTypeCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{ApiBaseUrl}/ShipmentTypes/create-shipment-type", dto);

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto<ShipmentTypeCreateDto>>() ?? new();

        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync(
            $"{ApiBaseUrl}/ShipmentTypes/delete-shipment-type/{id}");

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto>() ?? new();

        if (!response.IsSuccessStatusCode || !result.IsSucceed)
            return NotFound(result.Message);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var response = await _httpClient.GetAsync(
            $"{ApiBaseUrl}/ShipmentTypes/get-update-dto/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto<ShipmentTypeUpdateDto>>() ?? new();

        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ShipmentTypeUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{ApiBaseUrl}/ShipmentTypes/update-shipment-type", dto);

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto>() ?? new();

        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

        return RedirectToAction("Index");
    }
}
