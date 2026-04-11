using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CountryDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _service;

        public CountriesController(ICountryService service)
        {
            _service = service;
        }

        [HttpGet("get-all-countries")]
        public async Task<IActionResult> GetAllCountries()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-country-by-id/{id}")]
        public async Task<IActionResult> GetCountryById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-country")]
        public async Task<IActionResult> CreateCountry([FromBody] CountryCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete-country/{id}")]
        public async Task<IActionResult> DeleteCountry([FromRoute] Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }


        [HttpPut("update-country")]
        public async Task<IActionResult> UpdateCountry([FromBody] CountryUpdateDto dto)
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
