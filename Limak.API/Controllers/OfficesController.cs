using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.OfficeDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _service;

        public OfficesController(IOfficeService service)
        {
            _service = service;
        }

        [HttpGet("get-all-offices")]
        public async Task<IActionResult> GetAllOffices()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-office-by-id/{id}")]
        public async Task<IActionResult> GetOfficeById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-office")]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete-office/{id}")]
        public async Task<IActionResult> DeleteOffice([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut("update-office")]
        public async Task<IActionResult> UpdateOffice([FromBody] OfficeUpdateDto dto)
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
