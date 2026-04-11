using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IDeliveryTypeService
{
    Task<ResultDto> CreateAsync(DeliveryTypeCreateDto dto);
    Task<ResultDto> UpdateAsync(DeliveryTypeUpdateDto dto);
    Task<ResultDto<DeliveryTypeUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<DeliveryTypeGetDto>>> GetAllAsync();
    Task<ResultDto<DeliveryTypeGetDto>> GetByIdAsync(Guid id);
}
