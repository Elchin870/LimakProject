using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.OrderDtos;
using Limak.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public UserOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private Guid GetCustomerId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost("create-order/{id}")]
        public async Task<IActionResult> CreateOrder([FromRoute] Guid id, [FromBody] UserOrderCreateDto dto)
        {
            var result = await _orderService.CreateAsync(id, dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var customerId = GetCustomerId();
            var result = await _orderService.GetUserOrderAsync(customerId, id);
            return Ok(result);
        }

        [HttpGet("get-all-orders/{id}")]
        public async Task<IActionResult> GetAllOrders(Guid id)
        {
            var result = await _orderService.GetUserOrdersAsync(id);
            return Ok(result);
        }

        [HttpGet("filtered/{customerId}")]
        public async Task<IActionResult> GetFilteredOrders(
            [FromRoute] Guid customerId,
            [FromQuery] Guid? countryId,
            [FromQuery] OrderStatus? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            var result = await _orderService.GetUserOrdersFilteredAsync(customerId, countryId, status, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
