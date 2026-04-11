using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.MVC.ViewModels.SettingsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;
[Authorize(Roles = "Member")]
public class SettingsController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var updateModel = await GetUpdateCustomerModel();
        var passwordModel = new ChangePasswordViewModel();

        ViewBag.PasswordModel = passwordModel;

        return View(updateModel);
    }

    private async Task<UpdateCustomerViewModel> GetUpdateCustomerModel()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var customerResponse =await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-update-dto/{userId}");

        var customerResult =await customerResponse.Content.ReadFromJsonAsync<ResultDto<CustomerUpdateDto>>();

        var officeResponse =await _httpClient.GetAsync($"{ApiBaseUrl}/Offices/get-all-offices");

        var officeResult =await officeResponse.Content.ReadFromJsonAsync<ResultDto<List<OfficeGetDto>>>();

        return new UpdateCustomerViewModel
        {
            Customer = customerResult!.Data!,
            Offices = officeResult!.Data!
        };
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCustomer(UpdateCustomerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Offices = (await GetUpdateCustomerModel()).Offices;
            ViewBag.PasswordModel = new ChangePasswordViewModel();
            return View("Index", model);
        }

        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/Customers/update-customer",model.Customer);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Update failed");
            model.Offices = (await GetUpdateCustomerModel()).Offices;
            ViewBag.PasswordModel = new ChangePasswordViewModel();
            return View("Index", model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var updateModel = await GetUpdateCustomerModel();
            ViewBag.PasswordModel = model;
            return View("Index", updateModel);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var customerResponse =await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-update-dto/{userId}");

        var customerResult =await customerResponse.Content.ReadFromJsonAsync<ResultDto<CustomerUpdateDto>>();

        model.Password.CustomerId = customerResult!.Data!.Id;

        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Customers/change-password",model.Password);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Password change failed");
            var updateModel = await GetUpdateCustomerModel();
            ViewBag.PasswordModel = model;
            return View("Index", updateModel);
        }

        return RedirectToAction(nameof(Index));
    }
}
