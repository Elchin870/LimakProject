using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Application.Dtos.PartnerDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PartnerController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var partnerResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Partners/get-all-partners");
        if (!partnerResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var partnerList = await partnerResponse.Content.ReadFromJsonAsync<ResultDto<List<PartnerGetDto>>>() ?? new();
        var model = partnerList.Data ?? new List<PartnerGetDto>();

        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResponse = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Partners/delete-partner/{id}");
        var deleteResult = await deleteResponse.Content.ReadFromJsonAsync<ResultDto>() ?? new();
        if (!deleteResult.IsSucceed)
        {
            return NotFound(deleteResult.Message);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PartnerCreateDto partnerCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(partnerCreateDto);
        }

        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(partnerCreateDto.Name), "Name");
            content.Add(new StringContent(partnerCreateDto.WebsitePath), "WebsitePath");
            if (partnerCreateDto.Image != null)
            {
                var imageContent = new StreamContent(partnerCreateDto.Image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(partnerCreateDto.Image.ContentType);
                content.Add(imageContent, "Image", partnerCreateDto.Image.FileName);
            }

            var response = await _httpClient.PostAsync($"{ApiBaseUrl}/Partners/create-partner", content);
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                return View(partnerCreateDto);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var updatedPartner = await _httpClient.GetAsync($"{ApiBaseUrl}/Partners/get-update-dto/{id}");
        if (!updatedPartner.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var partnerResult = await updatedPartner.Content.ReadFromJsonAsync<ResultDto<PartnerUpdateDto>>() ?? new();

        return View(partnerResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PartnerUpdateDto partnerUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(partnerUpdateDto);
        }
        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(partnerUpdateDto.Id.ToString()), "Id");
            content.Add(new StringContent(partnerUpdateDto.Name), "Name");
            content.Add(new StringContent(partnerUpdateDto.WebsitePath), "WebsitePath");
            if (partnerUpdateDto.Image != null)
            {
                var imageContent = new StreamContent(partnerUpdateDto.Image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(partnerUpdateDto.Image.ContentType);
                content.Add(imageContent, "Image", partnerUpdateDto.Image.FileName);
            }
            var response = await _httpClient.PutAsync($"{ApiBaseUrl}/Partners/update-partner", content);
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                return View(partnerUpdateDto);
            }
        }
        return RedirectToAction("Index");
    }
}
