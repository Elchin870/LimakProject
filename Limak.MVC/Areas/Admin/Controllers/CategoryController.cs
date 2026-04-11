using Limak.Application.Dtos.CategoryDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var categoryResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-all-categories");

        if (!categoryResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var categoryList = await categoryResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();

        return View(categoryList.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Categories/create-category", categoryCreateDto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto<CategoryCreateDto>>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(categoryCreateDto);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Categories/delete-category/{id}");
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
        var updatedCategory = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-update-dto/{id}");

        if (!updatedCategory.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var result = await updatedCategory.Content.ReadFromJsonAsync<ResultDto<CategoryUpdateDto>>() ?? new();
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Categories/update-category", categoryUpdateDto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);
            return View(categoryUpdateDto);
        }
        return RedirectToAction("Index");
    }
}
