using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.TariffDtos;

namespace Limak.Application.Abstractions.Services;

public interface ITariffService
{
    Task<ResultDto> CreateAsync(TariffCreateDto dto);
    Task<ResultDto> UpdateAsync(TariffUpdateDto dto);
    Task<ResultDto<TariffUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<TariffGetDto>>> GetAllAsync();
    Task<ResultDto<TariffGetDto>> GetByIdAsync(Guid id);
}
