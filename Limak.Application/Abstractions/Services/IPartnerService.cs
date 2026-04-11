using Limak.Application.Dtos.PartnerDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IPartnerService
{
    Task<ResultDto> CreateAsync(PartnerCreateDto dto);
    Task<ResultDto> UpdateAsync(PartnerUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<PartnerUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto<List<PartnerGetDto>>> GetAllAsync();
    Task<ResultDto<PartnerGetDto>> GetByIdAsync(Guid id);
}
