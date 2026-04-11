using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.KargomatDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KargomatsController : ControllerBase
    {
        private readonly IKargomatService _service;

        public KargomatsController(IKargomatService service)
        {
            _service = service;
        }

        [HttpGet("get-all-kargomats")]
        public async Task<IActionResult> GetAllKargomats()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-kargomat-by-id/{id}")]
        public async Task<IActionResult> GetKargomatById([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-kargomat")]
        public async Task<IActionResult> AddKargomat([FromBody] KargomatCreateDto kargomatDto)
        {
            var result = await _service.CreateAsync(kargomatDto);
            return Ok(result);
        }

        [HttpPut("update-kargomat")]
        public async Task<IActionResult> UpdateKargomat([FromBody] KargomatUpdateDto kargomatDto)
        {
            var result = await _service.UpdateAsync(kargomatDto);
            return Ok(result);
        }

        [HttpDelete("delete-kargomat/{id}")]
        public async Task<IActionResult> DeleteKargomat([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
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
