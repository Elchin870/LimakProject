using Limak.Domain.Enums;

namespace Limak.Application.Dtos.OrderDtos;

public class UserOrderCreateDto
{
    public Guid CountryId { get; set; }
    public string StoreName { get; set; }
    public string StoreUrl { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAzn { get; set; }
}
