using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.TariffDtos;
using Limak.MVC.ViewModels.TariffViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;

public class TariffController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var tariffsReponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Tariffs/get-all-tariffs");
        var tariffList = await tariffsReponse.Content.ReadFromJsonAsync<ResultDto<List<TariffGetDto>>>() ?? new();

        if (!tariffsReponse.IsSuccessStatusCode || !tariffList.IsSucceed)
        {
            return BadRequest(tariffList.Message);
        }

        var model = new TariffViewModel
        {
            Tariffs = tariffList.Data!
        };

        return View(model);
    }
}
