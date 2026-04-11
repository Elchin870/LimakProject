namespace Limak.Application.Dtos.PurchaseDtos;

public class PurchaseCreateDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Description { get; set; }
    public string RedirectUrl { get; set; }
}
