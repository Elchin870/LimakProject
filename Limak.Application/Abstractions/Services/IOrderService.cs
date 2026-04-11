using Limak.Application.Dtos.OrderDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Domain.Enums;

namespace Limak.Application.Abstractions.Services;

public interface IOrderService
{
    Task<ResultDto> CreateAsync(Guid customerId, UserOrderCreateDto dto);

    Task<ResultDto<UserOrderGetDto>> GetUserOrderAsync(Guid customerId, Guid orderId);
    Task<ResultDto<List<UserOrderGetDto>>> GetUserOrdersAsync(Guid customerId);
    Task<ResultDto<PaginatedUserOrdersDto>> GetUserOrdersFilteredAsync(Guid customerId, Guid? countryId, OrderStatus? status, int pageNumber = 1, int pageSize = 5);

    Task<ResultDto<List<AdminOrderGetDto>>> GetAllAsync();

    Task<ResultDto> ReviewAsync(string adminUserId, AdminOrderReviewDto dto);
}
