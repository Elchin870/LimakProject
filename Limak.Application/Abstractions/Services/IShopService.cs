using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShopDtos;

namespace Limak.Application.Abstractions.Services;

public interface IShopService
{
    Task<ResultDto> CreateAsync(ShopCreateDto dto);
    Task<ResultDto> UpdateAsync(ShopUpdateDto dto);
    Task<ResultDto<ShopUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<ShopGetDto>>> GetAllAsync();
    Task<ResultDto<ShopGetDto>> GetByIdAsync(Guid id);
}
