using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.ShipmentTypeDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentTypesController : ControllerBase
    {
        private readonly IShipmentTypeService _service;

        public ShipmentTypesController(IShipmentTypeService service)
        {
            _service = service;
        }

        [HttpGet("get-all-shipment-types")]
        public async Task<IActionResult> GetAllShipmentTypes()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-shipment-type-by-id/{id}")]
        public async Task<IActionResult> GetShipmentTypeById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-shipment-type")]
        public async Task<IActionResult> CreateShipmentType([FromBody] ShipmentTypeCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete-shipment-type/{id}")]
        public async Task<IActionResult> DeleteShipmentType([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut("update-shipment-type")]
        public async Task<IActionResult> UpdateShipmentType([FromBody] ShipmentTypeUpdateDto dto)
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
