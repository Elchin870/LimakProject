using Limak.Application.Dtos.CountryAddressDtos;
using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CountryAddressController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var addressResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/CountryAddresses/get-all-addresses");

        if (!addressResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var addressList = await addressResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryAddressGetDto>>>() ?? new();
        var model = addressList.Data ?? new();
        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResponse = await _httpClient.DeleteAsync($"{ApiBaseUrl}/CountryAddresses/delete-address/{id}");
        var deleteResult = await deleteResponse.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!deleteResult.IsSucceed || !deleteResult.IsSucceed)
        {
            return NotFound(deleteResult.Message);
        }

        return RedirectToAction("Index");
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

        ViewBag.Countries = countries;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CountryAddressCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/CountryAddresses/create-address", dto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto<CountryAddressCreateDto>>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);

            var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
            var countries = new List<CountryGetDto>();
            if (countriesResponse.IsSuccessStatusCode)
            {
                var countryList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();
                countries = countryList.Data ?? new();
            }
            ViewBag.Countries = countries;

            return View(dto);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var updatedAddress = await _httpClient.GetAsync($"{ApiBaseUrl}/CountryAddresses/get-update-dto/{id}");

        if (!updatedAddress.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var result = await updatedAddress.Content.ReadFromJsonAsync<ResultDto<CountryAddressUpdateDto>>() ?? new();

        var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
        var countries = new List<CountryGetDto>();
        if (countriesResponse.IsSuccessStatusCode)
        {
            var countryList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();
            countries = countryList.Data ?? new();
        }

        ViewBag.Countries = countries;
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CountryAddressUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/CountryAddresses/update-address", dto);
        var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!response.IsSuccessStatusCode || !result.IsSucceed)
        {
            ModelState.AddModelError("", result.Message);

            var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
            var countries = new List<CountryGetDto>();
            if (countriesResponse.IsSuccessStatusCode)
            {
                var countryList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();
                countries = countryList.Data ?? new();
            }
            ViewBag.Countries = countries;

            return View(dto);
        }
        return RedirectToAction("Index");
    }
}
