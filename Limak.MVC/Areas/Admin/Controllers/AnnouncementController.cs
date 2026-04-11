using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AnnouncementController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index()
    {
        var announcementsResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Announcements/get-all-announcements");
        if (!announcementsResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var announcementsList = await announcementsResponse.Content.ReadFromJsonAsync<ResultDto<List<AnnouncementGetDto>>>() ?? new();
        var model = announcementsList.Data ?? new List<AnnouncementGetDto>();
        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResponse = await _httpClient.DeleteAsync($"{ApiBaseUrl}/Announcements/delete-announcement/{id}");
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
    public async Task<IActionResult> Create(AnnouncementCreateDto announcementCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(announcementCreateDto);
        }

        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(announcementCreateDto.Title), "Title");
            content.Add(new StringContent(announcementCreateDto.Description), "Description");
            if (announcementCreateDto.Image != null)
            {
                var imageContent = new StreamContent(announcementCreateDto.Image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(announcementCreateDto.Image.ContentType);
                content.Add(imageContent, "Image", announcementCreateDto.Image.FileName);
            }

            var response = await _httpClient.PostAsync($"{ApiBaseUrl}/Announcements/create-announcement", content);
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                return View(announcementCreateDto);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var updatedAnnouncement = await _httpClient.GetAsync($"{ApiBaseUrl}/Announcements/get-update-dto/{id}");
        if (!updatedAnnouncement.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var announcementResult = await updatedAnnouncement.Content.ReadFromJsonAsync<ResultDto<AnnouncementUpdateDto>>() ?? new();

        return View(announcementResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(AnnouncementUpdateDto announcementUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(announcementUpdateDto);
        }
        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(announcementUpdateDto.Id.ToString()), "Id");
            content.Add(new StringContent(announcementUpdateDto.Title), "Title");
            content.Add(new StringContent(announcementUpdateDto.Description), "Description");
            if (announcementUpdateDto.Image != null)
            {
                var imageContent = new StreamContent(announcementUpdateDto.Image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(announcementUpdateDto.Image.ContentType);
                content.Add(imageContent, "Image", announcementUpdateDto.Image.FileName);
            }
            var response = await _httpClient.PutAsync($"{ApiBaseUrl}/Announcements/update-announcement", content);
            var result = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Message);
                return View(announcementUpdateDto);
            }
        }
        return RedirectToAction("Index");
    }
}
