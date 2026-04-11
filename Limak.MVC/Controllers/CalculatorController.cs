using Limak.Application.Dtos.CalcullatorDtos;
using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShipmentTypeDtos;
using Limak.MVC.ViewModels.CalcullatorViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers
{
    public class CalculatorController(IHttpClientFactory _httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
        private const string ApiBaseUrl = "https://localhost:7078/api";
        public async Task<IActionResult> Index()
        {
            var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
            var countriesList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();

            if (!countriesResponse.IsSuccessStatusCode || !countriesList.IsSucceed)
            {
                return BadRequest(countriesList.Message);
            }

            var deliveryTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/DeliveryTypes/get-all-delivery-types");
            var deliveryTypesList = await deliveryTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<DeliveryTypeGetDto>>>() ?? new();

            if (!deliveryTypesResponse.IsSuccessStatusCode || !deliveryTypesList.IsSucceed)
            {
                return BadRequest(deliveryTypesList.Message);
            }

            var shipmentTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/ShipmentTypes/get-all-shipment-types");
            var shipmentTypesList = await shipmentTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<ShipmentTypeGetDto>>>() ?? new();

            if (!shipmentTypesResponse.IsSuccessStatusCode || !shipmentTypesList.IsSucceed)
            {
                return BadRequest(shipmentTypesList.Message);
            }

            var model = new CalculatorViewModel
            {
                Calculator = new ShipmentCalculateDto(),
                Countries = countriesList.Data!,
                ShipmentTypes = shipmentTypesList.Data!,
                DeliveryTypes = deliveryTypesList.Data!,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(CalculatorViewModel model)
        {
            var countriesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Countries/get-all-countries");
            var countriesList = await countriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CountryGetDto>>>() ?? new();

            if (!countriesResponse.IsSuccessStatusCode || !countriesList.IsSucceed)
            {
                return BadRequest(countriesList.Message);
            }

            var deliveryTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/DeliveryTypes/get-all-delivery-types");
            var deliveryTypesList = await deliveryTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<DeliveryTypeGetDto>>>() ?? new();

            if (!deliveryTypesResponse.IsSuccessStatusCode || !deliveryTypesList.IsSucceed)
            {
                return BadRequest(deliveryTypesList.Message);
            }

            var shipmentTypesResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/ShipmentTypes/get-all-shipment-types");
            var shipmentTypesList = await shipmentTypesResponse.Content.ReadFromJsonAsync<ResultDto<List<ShipmentTypeGetDto>>>() ?? new();

            if (!shipmentTypesResponse.IsSuccessStatusCode || !shipmentTypesList.IsSucceed)
            {
                return BadRequest(shipmentTypesList.Message);
            }


            if (!ModelState.IsValid)
            {
                model.Countries = countriesList.Data!;
                model.DeliveryTypes = deliveryTypesList.Data!;
                model.ShipmentTypes = shipmentTypesList.Data!;
                return View("Index", model);
            }

            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Calculators/calculate-shipment", model.Calculator);
            var result = await response.Content.ReadFromJsonAsync<ResultDto<decimal>>() ?? new();

            if (!response.IsSuccessStatusCode || !result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                model.Countries = countriesList.Data!;
                model.DeliveryTypes = deliveryTypesList.Data!;
                model.ShipmentTypes = shipmentTypesList.Data!;

                return View("Index", model);
            }

            model.TotalPrice = result.Data;
            model.Countries = countriesList.Data!;
            model.DeliveryTypes = deliveryTypesList.Data!;
            model.ShipmentTypes = shipmentTypesList.Data!;
            return View("Index", model);
        }
    }
}
