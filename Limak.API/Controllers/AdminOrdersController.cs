using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.OrderDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
       

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("review/{id}")]
        public async Task<IActionResult> Review(string id, [FromBody] AdminOrderReviewDto dto)
        {
            var result = await _orderService.ReviewAsync(id, dto);
            return Ok(result);
        }
    }
}
