using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.BalanceTransactionDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceTransactionsController : ControllerBase
    {
        private readonly IBalanceTransactionService _balanceTransactionService;

        public BalanceTransactionsController(IBalanceTransactionService balanceTransactionService)
        {
            _balanceTransactionService = balanceTransactionService;
        }

        private Guid GetCustomerId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _balanceTransactionService.GetByCustomerAsync(GetCustomerId());
            return Ok(result);
        }

        [HttpGet("get-all-admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var result = await _balanceTransactionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-all-user/{id}")]
        public async Task<IActionResult> GetAllUser(Guid id)
        {
            var result = await _balanceTransactionService.GetByCustomerIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-balance-transaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] BalanceTransactionCreateDto dto)
        {
            var result = await _balanceTransactionService.CreateAsync(dto);
            return Ok(result);
        }
    }
}
