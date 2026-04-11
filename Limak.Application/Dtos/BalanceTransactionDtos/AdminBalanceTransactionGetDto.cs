using Limak.Domain.Enums;

namespace Limak.Application.Dtos.BalanceTransactionDtos;

public class AdminBalanceTransactionGetDto
{
    public Guid Id { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string CustomerCode { get; set; }
    public BalanceTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public Guid? OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
}
