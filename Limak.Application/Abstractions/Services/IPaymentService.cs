using Limak.Application.Dtos.PaymentDtos;
using Limak.Application.Dtos.PurchaseDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface IPaymentService
{
    Task<ResultDto<PurchaseGetDto>> CreatePaymentRequest(PurchaseCreateDto dto);
    Task<ResultDto<PurchaseDetailDto>> GetPurchaseInfoAsync(int id);
    Task<ResultDto> CreateAsync(PaymentCreateDto dto);
    Task<ResultDto<PaymentGetDto>> GetAsync(int id);
    Task<ResultDto<PaymentGetDto>> GetByIdAsync(Guid id);
    Task<ResultDto> UpdateAsync(PaymentUpdateDto dto);
    Task<ResultDto<PaymentGetDto>> GetPaymentByAppUserId(string appUserId);
}
