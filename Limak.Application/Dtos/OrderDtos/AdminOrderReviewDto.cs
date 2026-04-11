using Limak.Domain.Enums;

namespace Limak.Application.Dtos.OrderDtos;

public class AdminOrderReviewDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public string? AdminNote { get; set; }
    
}
