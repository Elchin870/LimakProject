using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.TariffDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TariffsController : ControllerBase
    {
        private readonly ITariffService _service;

        public TariffsController(ITariffService service)
        {
            _service = service;
        }

        [HttpGet("get-all-tariffs")]
        public async Task<IActionResult> GetAllTariffs()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-tariff-by-id/{id}")]
        public async Task<IActionResult> GetTariffById([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }


        [HttpPost("create-tariff")]
        public async Task<IActionResult> CreateCategory([FromBody] TariffCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete-tariff/{id}")]
        public async Task<IActionResult> DeleteTariff([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut("update-tariff")]
        public async Task<IActionResult> UpdateTariff([FromBody] TariffUpdateDto dto)
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
