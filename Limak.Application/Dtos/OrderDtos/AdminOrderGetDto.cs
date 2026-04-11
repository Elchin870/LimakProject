using Limak.Domain.Enums;

namespace Limak.Application.Dtos.OrderDtos;

public class AdminOrderGetDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string CustomerCode { get; set; }
    public string CountryName { get; set; }
    public string StoreName { get; set; }
    public string StoreUrl { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAzn { get; set; }
    public OrderStatus Status { get; set; }
    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? AdminNote { get; set; }
    public DateTime CreatedAt { get; set; }
}
