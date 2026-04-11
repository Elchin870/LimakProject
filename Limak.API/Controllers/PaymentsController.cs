using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.PaymentDtos;
using Limak.Application.Dtos.PurchaseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost("create-purchase")]
        public async Task<IActionResult> CreatePurchase([FromBody] PurchaseCreateDto dto)
        {
            var result = await _service.CreatePaymentRequest(dto);
            return Ok(result);
        }

        [HttpGet("get-purchase-info/{id}")]
        public async Task<IActionResult> GetPurchaseInfo([FromRoute] int id)
        {
            var result = await _service.GetPurchaseInfoAsync(id);
            return Ok(result);
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("get-payment/{id}")]
        public async Task<IActionResult> GetPayment([FromRoute] int id)
        {
            var result = await _service.GetAsync(id);
            return Ok(result);
        }

        [HttpPut("update-payment")]
        public async Task<IActionResult> UpdatePayment([FromBody] PaymentUpdateDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpGet("get-payment-by-appuser-id/{appUserId}")]
        public async Task<IActionResult> GetPaymentByAppUserId([FromRoute] string appUserId)
        {
            var result = await _service.GetPaymentByAppUserId(appUserId);
            return Ok(result);
        }
    }
}
