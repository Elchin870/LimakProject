using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CustomerDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet("get-customer-by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("get-customer/{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] string id)
        {
            var result = await _service.GetAsync(id);
            return Ok(result);
        }

        [HttpGet("get-update-dto/{appUserId}")]
        public async Task<IActionResult> GetUpdateDto([FromRoute] string appUserId)
        {
            var result = await _service.GetUpdateDto(appUserId);
            return Ok(result);
        }

        [HttpPut("update-customer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdateDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpPost("increase-balance")]
        public async Task<IActionResult> IncreaseBalance([FromBody] IncreaseBalanceDto dto)
        {
            var result = await _service.IncreaseBalance(dto.CustomerId, dto.Amount);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var result = await _service.ChangePassword(dto);
            return Ok(result);
        }
    }
}
