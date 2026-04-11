using Limak.Domain.Entities;
using Limak.Domain.Enums;

namespace Limak.Application.Dtos.PaymentDtos;

public class PaymentCreateDto
{
    public int PurchaseId { get; set; }
    public string Password { get; set; }
    public string Secret { get; set; }
    public decimal Amount { get; set; }
    public string AppUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentStatuses PaymentStatus { get; set; }
}
