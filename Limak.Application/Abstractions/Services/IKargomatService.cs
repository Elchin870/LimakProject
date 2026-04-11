using Limak.Application.Dtos.KargomatDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IKargomatService
{
    Task<ResultDto> CreateAsync(KargomatCreateDto dto);
    Task<ResultDto> UpdateAsync(KargomatUpdateDto dto);
    Task<ResultDto<KargomatUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<KargomatGetDto>>> GetAllAsync();
    Task<ResultDto<KargomatGetDto>> GetByIdAsync(Guid id);
}
