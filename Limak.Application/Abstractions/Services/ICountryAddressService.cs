using Limak.Application.Dtos.CountryAddressDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface ICountryAddressService
{
    Task<ResultDto> CreateAsync(CountryAddressCreateDto dto);
    Task<ResultDto> UpdateAsync(CountryAddressUpdateDto dto);
    Task<ResultDto<CountryAddressUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<CountryAddressGetDto>>> GetAllAsync();
    Task<ResultDto<CountryAddressGetDto>> GetByIdAsync(Guid id);
}
