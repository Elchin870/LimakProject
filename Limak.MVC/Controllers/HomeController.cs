using Limak.Application.Dtos.AccountDtos;
using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.PartnerDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShopDtos;
using Limak.Application.Dtos.TariffDtos;
using Limak.MVC.Models;
using Limak.MVC.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;

public class HomeController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var shopsReponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Shops/get-all-shops");
        var shopList = await shopsReponse.Content.ReadFromJsonAsync<ResultDto<List<ShopGetDto>>>() ?? new();

        if (!shopsReponse.IsSuccessStatusCode || !shopList.IsSucceed)
        {
            return BadRequest(shopList.Message);
        }

        var announcementsReponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Announcements/get-all-announcements");
        var announcementList = await announcementsReponse.Content.ReadFromJsonAsync<ResultDto<List<AnnouncementGetDto>>>() ?? new();

        if (!announcementsReponse.IsSuccessStatusCode || !announcementList.IsSucceed)
        {
            return BadRequest(announcementList.Message);
        }

        var tariffsReponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Tariffs/get-all-tariffs");
        var tariffList = await tariffsReponse.Content.ReadFromJsonAsync<ResultDto<List<TariffGetDto>>>() ?? new();

        if (!tariffsReponse.IsSuccessStatusCode || !tariffList.IsSucceed)
        {
            return BadRequest(tariffList.Message);
        }

        var partnersReponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Partners/get-all-partners");
        var partnerList = await partnersReponse.Content.ReadFromJsonAsync<ResultDto<List<PartnerGetDto>>>() ?? new();

        if (!partnersReponse.IsSuccessStatusCode || !partnerList.IsSucceed)
        {
            return BadRequest(partnerList.Message);
        }

        var officeResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Offices/get-all-offices");
        var officeList = await officeResponse.Content.ReadFromJsonAsync<ResultDto<List<OfficeGetDto>>>() ?? new();

        if (!officeResponse.IsSuccessStatusCode || !officeList.IsSucceed)
        {
            return BadRequest(officeList.Message);
        }


        var model = new HomeViewModel
        {
            Register = new RegisterDto(),
            Offices = officeList.Data!,
            Shops = shopList.Data!,
            Announcements = announcementList.Data!,
            Tariffs = tariffList.Data!,
            Partners = partnerList.Data!,
        };

        return View(model);
    }
}
