using Limak.Domain.Entities.Common;
using Limak.Domain.Enums;

namespace Limak.Domain.Entities;

public class BalanceTransaction : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public BalanceTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }
}
