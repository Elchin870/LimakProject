using Limak.Domain.Enums;

namespace Limak.Application.Dtos.BalanceTransactionDtos;

public class BalanceTransactionCreateDto
{
    public Guid CustomerId { get; set; }
    public BalanceTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? OrderId { get; set; }
}
