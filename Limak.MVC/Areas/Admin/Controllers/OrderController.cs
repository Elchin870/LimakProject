using Limak.Application.Dtos.OrderDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrderController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {

        var orderResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/AdminOrders/get-all");
        var orderList = await orderResponse.Content.ReadFromJsonAsync<ResultDto<List<AdminOrderGetDto>>>() ?? new();

        var model = orderList.Data ?? new List<AdminOrderGetDto>();
        return View(model);
    }
    

    [HttpPost]
    public async Task<IActionResult> Review(AdminOrderReviewDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reviewResponse = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/AdminOrders/review/{userId}", dto);
        var reviewResult = await reviewResponse.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!reviewResponse.IsSuccessStatusCode || !reviewResult.IsSucceed)
        {
            TempData["ErrorMessage"] = reviewResult.Message;
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Order reviewed successfully!";
        return RedirectToAction("Index");
    }
}
