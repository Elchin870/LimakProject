using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CalcullatorDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorsController : ControllerBase
    {
        private readonly IShipmentCalculatorService _service;

        public CalculatorsController(IShipmentCalculatorService service)
        {
            _service = service;
        }

        [HttpPost("calculate-shipment")]
        public async Task<ActionResult> CalculateShipment([FromBody] ShipmentCalculateDto dto)
        {
            var result = await _service.CalculatePriceAsync(dto);
            return Ok(result);
        }
    }
}
