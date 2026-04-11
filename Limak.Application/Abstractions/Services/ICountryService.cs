using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface ICountryService
{
    Task<ResultDto> CreateAsync(CountryCreateDto dto);
    Task<ResultDto> UpdateAsync(CountryUpdateDto dto);
    Task<ResultDto<CountryUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<CountryGetDto>>> GetAllAsync();
    Task<ResultDto<CountryGetDto>> GetByIdAsync(Guid id);
}
