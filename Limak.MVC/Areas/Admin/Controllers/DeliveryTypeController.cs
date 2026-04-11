using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DeliveryTypeController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient
            .GetAsync($"{ApiBaseUrl}/DeliveryTypes/get-all-delivery-types");

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        var list = await response.Content
            .ReadFromJsonAsync<ResultDto<List<DeliveryTypeGetDto>>>() ?? new();

        return View(list.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(DeliveryTypeCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{ApiBaseUrl}/DeliveryTypes/create-delivery-type", dto);

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto<DeliveryTypeCreateDto>>() ?? new();

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
            $"{ApiBaseUrl}/DeliveryTypes/delete-delivery-type/{id}");

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
            $"{ApiBaseUrl}/DeliveryTypes/get-update-dto/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto<DeliveryTypeUpdateDto>>() ?? new();

        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(DeliveryTypeUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{ApiBaseUrl}/DeliveryTypes/update-delivery-type", dto);

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
