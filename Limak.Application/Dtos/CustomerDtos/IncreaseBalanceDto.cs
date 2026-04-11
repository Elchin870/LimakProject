namespace Limak.Application.Dtos.CustomerDtos;

public class IncreaseBalanceDto
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}
