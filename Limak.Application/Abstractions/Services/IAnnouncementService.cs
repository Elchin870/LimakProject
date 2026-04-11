using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IAnnouncementService
{
    Task<ResultDto> CreateAsync(AnnouncementCreateDto dto);
    Task<ResultDto> UpdateAsync(AnnouncementUpdateDto dto);
    Task<ResultDto<AnnouncementUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<AnnouncementGetDto>>> GetAllAsync();
    Task<ResultDto<AnnouncementGetDto>> GetByIdAsync(Guid id);
}
