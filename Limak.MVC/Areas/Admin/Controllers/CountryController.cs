using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CountryController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        var countryList = await response.Content
            .ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();

        return View(countryList.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CountryCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{ApiBaseUrl}/Countries/create-country", dto);

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto<CountryCreateDto>>() ?? new();

        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient
            .DeleteAsync($"{ApiBaseUrl}/Countries/delete-country/{id}");

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto>() ?? new();

        if (!response.IsSuccessStatusCode || !result.IsSucceed)
            return NotFound(result.Message);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var response = await _httpClient
            .GetAsync($"{ApiBaseUrl}/Countries/get-update-dto/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        var result = await response.Content
            .ReadFromJsonAsync<ResultDto<CountryUpdateDto>>() ?? new();

        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CountryUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{ApiBaseUrl}/Countries/update-country", dto);

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
