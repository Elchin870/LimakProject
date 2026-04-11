using Limak.Application.Dtos.PackageDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.MVC.ViewModels.DeclareViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DeclareController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync($"{ApiBaseUrl}/AdminPackages/get-declared-packages");
        var result = await response.Content.ReadFromJsonAsync<ResultDto<List<AdminPackageListDto>>>() ?? new();

        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            return BadRequest(result.Message);
        }

        DeclareViewModelAdmin model = new DeclareViewModelAdmin()
        {
            Packages = result.Data!
        };
        return View(model);
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
