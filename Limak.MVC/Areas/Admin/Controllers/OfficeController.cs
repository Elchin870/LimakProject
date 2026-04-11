using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OfficeController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var officeResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Offices/get-all-offices");

        if (!officeResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var officeList = await officeResponse.Content.ReadFromJsonAsync<ResultDto<List<OfficeGetDto>>>() ?? new();

        return View(officeList.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(OfficeCreateDto officeCreateDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Offices/create-office", officeCreateDto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto<OfficeCreateDto>>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(officeCreateDto);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Offices/delete-office/{id}");
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
        var updatedOffice = await _httpClient.GetAsync($"{ApiBaseUrl}/Offices/get-update-dto/{id}");
        if (!updatedOffice.IsSuccessStatusCode)
        {
            return NotFound();
        }
        var result = await updatedOffice.Content.ReadFromJsonAsync<ResultDto<OfficeUpdateDto>>() ?? new();
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(OfficeUpdateDto officeUpdateDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Offices/update-office", officeUpdateDto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto<OfficeUpdateDto>>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(officeUpdateDto);
        }
        return RedirectToAction("Index");
    }

}
