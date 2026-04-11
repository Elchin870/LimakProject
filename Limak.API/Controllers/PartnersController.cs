using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.PartnerDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _service;

        public PartnersController(IPartnerService service)
        {
            _service = service;
        }

        [HttpGet("get-all-partners")]
        public async Task<IActionResult> GetAllPartners()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-partner-by-id/{id}")]
        public async Task<IActionResult> GetPartnerById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("delete-partner/{id}")]
        public async Task<IActionResult> DeletePartner([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost("create-partner")]
        public async Task<IActionResult> CreatePartner([FromForm] PartnerCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("update-partner")]
        public async Task<IActionResult> UpdatePartner([FromForm] PartnerUpdateDto dto)
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
