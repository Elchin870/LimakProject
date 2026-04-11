using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.AnnouncementDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _service;

        public AnnouncementsController(IAnnouncementService service)
        {
            _service = service;
        }

        [HttpGet("get-all-announcements")]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-announcement-by-id/{id}")]
        public async Task<IActionResult> GetAnnouncementById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("delete-announcement/{id}")]
        public async Task<IActionResult> DeleteAnnouncement([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost("create-announcement")]
        public async Task<IActionResult> CreateAnnouncement([FromForm] AnnouncementCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("update-announcement")]
        public async Task<IActionResult> UpdateAnnouncement([FromForm] AnnouncementUpdateDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpGet("get-update-dto/{id}")]
        public async Task<IActionResult> GetUpdateDto([FromRoute] Guid id)
        {
            var result = await _service.GetUpdateDto(id);
            return Ok(result);
        }
    }
}
