using Limak.Application.Dtos.KargomatDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class KargomatController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";

    public async Task<IActionResult> Index()
    {
        var kargomatResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Kargomats/get-all-kargomats");

        if (!kargomatResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var kargomatList = await kargomatResponse.Content.ReadFromJsonAsync<ResultDto<List<KargomatGetDto>>>() ?? new();

        return View(kargomatList.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(KargomatCreateDto kargomatCreateDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Kargomats/create-kargomat", kargomatCreateDto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(kargomatCreateDto);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Kargomats/delete-kargomat/{id}");
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            return NotFound(result.Message);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var updatedKargomat = await _httpClient.GetAsync($"{ApiBaseUrl}/Kargomats/get-update-dto/{id}");

        if (!updatedKargomat.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var result = await updatedKargomat.Content.ReadFromJsonAsync<ResultDto<KargomatUpdateDto>>() ?? new();
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(KargomatUpdateDto kargomatUpdateDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Kargomats/update-kargomat", kargomatUpdateDto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(kargomatUpdateDto);
        }
        return RedirectToAction("Index");
    }
}