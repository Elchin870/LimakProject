using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.PackageDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.MVC.ViewModels.PackageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PackageController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/AdminPackages/get-all-packages");
        var result = await response.Content.ReadFromJsonAsync<ResultDto<List<AdminPackageListDto>>>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            return BadRequest(result.Message);
        }

        AdminPackageViewModel model = new AdminPackageViewModel()
        {
            AdminPackageLists = result.Data ?? new List<AdminPackageListDto>()
        };


        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
        var countries = new List<CountryGetDto>();
        if (countriesResponse.IsSuccessStatusCode)
        {
            var countryList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();
            countries = countryList.Data ?? new();
        }

        ViewBag.Countries = countries.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdminCreatePackageDto dto)
    {
        var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
        var countries = new List<CountryGetDto>();
        if (countriesResponse.IsSuccessStatusCode)
        {
            var countryList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();
            countries = countryList.Data ?? new();
        }

        ViewBag.Countries = countries.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();
        if (!ModelState.IsValid)
        {
            return View(dto);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/AdminPackages/create-package/{userId}", dto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(dto);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(AdminPackageUpdateStatusDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/AdminPackages/update-status", dto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Package status updated successfully!";
        return RedirectToAction("Index");
    }
}
