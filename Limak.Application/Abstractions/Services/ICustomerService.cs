using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface ICustomerService
{
    Task<ResultDto> UpdateAsync(CustomerUpdateDto dto);
    Task<ResultDto<CustomerGetDto>> GetAsync(string id);
    Task<ResultDto<CustomerGetDto>> GetByIdAsync(Guid id);
    Task<ResultDto<CustomerUpdateDto>> GetUpdateDto(string appUserId);
    Task<ResultDto> IncreaseBalance(Guid id, decimal amount);
    Task<ResultDto> ChangePassword(ChangePasswordDto dto);
}
