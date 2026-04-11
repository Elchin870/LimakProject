using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShipmentTypeDtos;

namespace Limak.Application.Abstractions.Services;

public interface IShipmentTypeService
{
    Task<ResultDto> CreateAsync(ShipmentTypeCreateDto dto);
    Task<ResultDto> UpdateAsync(ShipmentTypeUpdateDto dto);
    Task<ResultDto<ShipmentTypeUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<ShipmentTypeGetDto>>> GetAllAsync();
    Task<ResultDto<ShipmentTypeGetDto>> GetByIdAsync(Guid id);
}
