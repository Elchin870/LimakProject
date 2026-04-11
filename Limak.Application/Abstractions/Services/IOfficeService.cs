using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IOfficeService
{
    Task<ResultDto> CreateAsync(OfficeCreateDto dto);
    Task<ResultDto> UpdateAsync(OfficeUpdateDto dto);
    Task<ResultDto<OfficeUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<OfficeGetDto>>> GetAllAsync();
    Task<ResultDto<OfficeGetDto>> GetByIdAsync(Guid id);
}
