using Limak.Application.Dtos.BalanceTransactionDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IBalanceTransactionService
{
    Task<ResultDto<List<AdminBalanceTransactionGetDto>>> GetAllAsync();

    Task<ResultDto<List<AdminBalanceTransactionGetDto>>> GetByCustomerAsync(Guid customerId);
    Task CreateAsync(InternalBalanceTransactionCreateDto dto);
    Task<ResultDto> CreateAsync(BalanceTransactionCreateDto dto);
    Task<ResultDto<List<BalanceTransactionGetDto>>> GetByCustomerIdAsync(Guid customerId);
}
