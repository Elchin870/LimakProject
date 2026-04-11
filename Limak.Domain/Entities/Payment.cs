using Limak.Domain.Entities.Common;
using Limak.Domain.Enums;

namespace Limak.Domain.Entities;

public class Payment : BaseEntity
{
    public int PurchaseId { get; set; }
    public string Password { get; set; }
    public string Secret { get; set; }
    public decimal Amount { get; set; }
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentStatuses PaymentStatus { get; set; }
}
