using Limak.Domain.Entities.Common;
using Limak.Domain.Enums;

namespace Limak.Domain.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public Guid CountryId { get; set; }
    public Country Country { get; set; }
    public string StoreName { get; set; }
    public string StoreUrl { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAzn { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? AdminNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public string OrderNumber { get; set; }
    public ICollection<BalanceTransaction> BalanceTransactions { get; set; } = new List<BalanceTransaction>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}
