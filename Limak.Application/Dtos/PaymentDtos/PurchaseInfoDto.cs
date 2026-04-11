using Limak.Domain.Enums;

namespace Limak.Application.Dtos.PurchaseDtos;

public class PurchaseInfoDto
{
    public int Id { get; set; }
    public string TypeRid { get; set; }
    public PaymentStatuses Status { get; set; }
    public string PrevStatus { get; set; }
    public string LastStatusLogin { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string CreateTime { get; set; }
    public PurchaseTypeDto Type { get; set; }
}
