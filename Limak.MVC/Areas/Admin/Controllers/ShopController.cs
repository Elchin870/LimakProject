using Limak.Application.Dtos.CategoryDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShopDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ShopController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var shopResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Shops/get-all-shops");
        if (!shopResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var shopList = await shopResponse.Content.ReadFromJsonAsync<ResultDto<List<ShopGetDto>>>() ?? new();
        var model = shopList.Data ?? new List<ShopGetDto>();
        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResponse = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Shops/delete-shop/{id}");
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
        var categoriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-all-categories");
        if (categoriesResponse.IsSuccessStatusCode)
        {
            var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
            ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
        }
        else
        {
            ViewBag.Categories = new List<CategoryGetDto>();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ShopCreateDto shopCreateDto)
    {
        if (!ModelState.IsValid)
        {
            var categoriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-all-categories");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
            }
            return View(shopCreateDto);
        }

        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(shopCreateDto.Name), "Name");
            content.Add(new StringContent(shopCreateDto.WebsitePath), "WebsitePath");
            content.Add(new StringContent(shopCreateDto.CategoryId.ToString()), "CategoryId");
            if (shopCreateDto.Image != null)
            {
                var imageContent = new StreamContent(shopCreateDto.Image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(shopCreateDto.Image.ContentType);
                content.Add(imageContent, "Image", shopCreateDto.Image.FileName);
            }
            var response = await _httpClient.PostAsync($"{ApiBaseUrl}/Shops/create-shop", content);
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                var categoriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-all-categories");
                if (categoriesResponse.IsSuccessStatusCode)
                {
                    var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                    ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
                }
                return View(shopCreateDto);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var updatedShop = await _httpClient.GetAsync($"{ApiBaseUrl}/Shops/get-update-dto/{id}");
        if (!updatedShop.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var shopResult = await updatedShop.Content.ReadFromJsonAsync<ResultDto<ShopUpdateDto>>() ?? new();

        var categoriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-all-categories");
        if (categoriesResponse.IsSuccessStatusCode)
        {
            var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
            ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
        }
        else
        {
            ViewBag.Categories = new List<CategoryGetDto>();
        }

        return View(shopResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ShopUpdateDto shopUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            var categoriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-all-categories");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
            }
            return View(shopUpdateDto);
        }
        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(shopUpdateDto.Id.ToString()), "Id");
            content.Add(new StringContent(shopUpdateDto.Name), "Name");
            content.Add(new StringContent(shopUpdateDto.WebsitePath), "WebsitePath");
            content.Add(new StringContent(shopUpdateDto.CategoryId.ToString()), "CategoryId");
            if (shopUpdateDto.Image != null)
            {
                var imageContent = new StreamContent(shopUpdateDto.Image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(shopUpdateDto.Image.ContentType);
                content.Add(imageContent, "Image", shopUpdateDto.Image.FileName);
            }
            var response = await _httpClient.PutAsync($"{ApiBaseUrl}/Shops/update-shop", content);
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                var categoriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Categories/get-all-categories");
                if (categoriesResponse.IsSuccessStatusCode)
                {
                    var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                    ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
                }
                return View(shopUpdateDto);
            }
        }
        return RedirectToAction("Index");
    }
}
