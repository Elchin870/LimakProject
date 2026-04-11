using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.ShopDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("get-all-shops")]
        public async Task<IActionResult> GetAllShops()
        {
            var result = await _shopService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-shop-by-id/{id}")]
        public async Task<IActionResult> GetShopById(Guid id)
        {
            var result = await _shopService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("create-shop")]
        public async Task<IActionResult> CreateShop([FromForm] ShopCreateDto dto)
        {
            var result = await _shopService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete-shop/{id}")]
        public async Task<IActionResult> DeleteShop([FromRoute] Guid id)
        {
            var result = await _shopService.DeleteAsync(id);
            return Ok(result);
        }


        [HttpPut("update-shop")]
        public async Task<IActionResult> UpdateShop([FromForm] ShopUpdateDto dto)
        {
            var result = await _shopService.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpGet("get-update-dto/{id}")]
        public async Task<IActionResult> GetUpdateDto([FromRoute] Guid id)
        {
            var result = await _shopService.GetUpdateDto(id);
            return Ok(result);

        }
    }
}
