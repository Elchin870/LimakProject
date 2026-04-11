using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CountryAddressDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryAddressesController : ControllerBase
    {
        private readonly ICountryAddressService _service;

        public CountryAddressesController(ICountryAddressService service)
        {
            _service = service;
        }

        [HttpGet("get-all-addresses")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-address-by-id/{id}")]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-address")]
        public async Task<IActionResult> CreateAddress([FromBody] CountryAddressCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete-address/{id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }


        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateAddress([FromBody] CountryAddressUpdateDto dto)
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
