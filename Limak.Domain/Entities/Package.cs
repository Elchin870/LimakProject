using Limak.Domain.Entities.Common;
using Limak.Domain.Enums;

namespace Limak.Domain.Entities;

public class Package : BaseEntity
{
    public Guid? OrderId { get; set; }
    public Order Order { get; set; }
    public Guid CountryId { get; set; }
    public Country Country { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public PackageStatus Status { get; set; }
    public int Quantity { get; set; }
    public string? Address { get; set; }
    public string? StoreName { get; set; }
    public string TrackingNumber { get; set; }
    public decimal? DeclaredPrice { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Width { get; set; }
    public decimal? Length { get; set; }
    public decimal? Height { get; set; }
    public string? AdminNote { get; set; }
    public decimal? ShippingPrice { get; set; }
    public Guid? DeliveryTypeId { get; set; }
    public DeliveryType DeliveryType { get; set; }
    public Guid? ShipmentTypeId { get; set; }
    public ShipmentType ShipmentType { get; set; }
    public Guid? KargomatId { get; set; }
    public Kargomat Kargomat { get; set; }
    public Office Office { get; set; }
    public Guid? OfficeId { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
