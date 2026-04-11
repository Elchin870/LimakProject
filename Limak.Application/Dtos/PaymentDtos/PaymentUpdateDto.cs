using Limak.Domain.Enums;

namespace Limak.Application.Dtos.PaymentDtos;

public class PaymentUpdateDto
{
    public Guid Id { get; set; }
    public int PurchaseId { get; set; }
    public string Password { get; set; }
    public string Secret { get; set; }
    public decimal Amount { get; set; }
    public string AppUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentStatuses PaymentStatus { get; set; }
}
