using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryTypesController : ControllerBase
    {
        private readonly IDeliveryTypeService _service;

        public DeliveryTypesController(IDeliveryTypeService service)
        {
            _service = service;
        }

        [HttpGet("get-all-delivery-types")]
        public async Task<IActionResult> GetAllDeliveryTypes()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-delivery-type-by-id/{id}")]
        public async Task<IActionResult> GetDeliveryTypeById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-delivery-type")]
        public async Task<IActionResult> CreateDeliveryType([FromBody] DeliveryTypeCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete-delivery-type/{id}")]
        public async Task<IActionResult> DeleteDeliveryType([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut("update-delivery-type")]
        public async Task<IActionResult> UpdateDeliveryType([FromBody] DeliveryTypeUpdateDto dto)
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
