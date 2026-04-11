using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShopDtos;
using Limak.MVC.ViewModels.ShopViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;

public class ShopController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var shopsResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Shops/get-all-shops");
        var shopList = await shopsResponse.Content.ReadFromJsonAsync<ResultDto<List<ShopGetDto>>>() ?? new();

        if (!shopsResponse.IsSuccessStatusCode || !shopList.IsSucceed)
        {
            return BadRequest(shopList.Message);
        }

        var model = new ShopViewModel
        {
            Shops = shopList.Data!
        };

        return View(model);
    }
}
