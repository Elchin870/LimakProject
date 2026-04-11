using Limak.Application.Dtos.AccountDtos;
using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.TokenDtos;
using Limak.Domain.Entities;
using Limak.MVC.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers
{
    public class AccountController(IHttpClientFactory _httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
        private const string ApiBaseUrl = "https://localhost:7078/api";
        public IActionResult Login()
        {
            var model = new LoginViewModel
            {
                Login = new LoginDto()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Auth/login", model.Login);
            var tokenResult = await response.Content.ReadFromJsonAsync<ResultDto<AccessTokenDto>>() ?? new();

            if (!response.IsSuccessStatusCode || !tokenResult.IsSucceed)
            {
                ModelState.AddModelError("", tokenResult.Message);
                return View(model);
            }


            Response.Cookies.Append("AccessToken", tokenResult.Data!.Token, new CookieOptions
            {
                HttpOnly = true,
                Expires = tokenResult.Data.ExpireDate
            });

            Response.Cookies.Append("RefreshToken", tokenResult.Data!.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = tokenResult.Data.RefreshTokenExpiredDate
            });

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenResult.Data.Token);
            var role = jwtToken.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;

            if (role == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }


            return RedirectToAction("Index", "UserPanel");
        }
        public async Task<IActionResult> Register()
        {
            var officeResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Offices/get-all-offices");
            var officeList = await officeResponse.Content.ReadFromJsonAsync<ResultDto<List<OfficeGetDto>>>() ?? new();

            if (!officeResponse.IsSuccessStatusCode || !officeList.IsSucceed)
            {
                return BadRequest(officeList.Message);
            }

            var model = new RegisterViewModel
            {
                Register = new RegisterDto(),
                Offices = officeList.Data!
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var officeResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Offices/get-all-offices");
            var officeList = await officeResponse.Content.ReadFromJsonAsync<ResultDto<List<OfficeGetDto>>>() ?? new();

            if (!officeResponse.IsSuccessStatusCode || !officeList.IsSucceed)
            {
                return BadRequest(officeList.Message);
            }

            model.Offices = officeList.Data!;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Auth/register", model.Register);
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!response.IsSuccessStatusCode || !result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }



            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/Auth/confirm-email?userId={userId}&token={token}");
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!response.IsSuccessStatusCode || !result.IsSucceed)
            {
                return View("ConfirmFail", result.Message);
            }
            return View("ConfirmSuccess", result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Request.Cookies.TryGetValue("RefreshToken", out var refreshToken);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _httpClient.PostAsync($"{ApiBaseUrl}/Auth/revoke-refresh-token?refreshToken={refreshToken}", null);
            }
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");

            return RedirectToAction("Index", "Home");
        }
    }
}
