using Limak.Domain.Enums;

namespace Limak.Application.Dtos.PackageDtos;

public class AdminPackageListDto
{
    public Guid Id { get; set; }
    public string CountryName { get; set; }
    public string OrderNumber { get; set; }
    public string TrackingNumber { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string CustomerCode { get; set; }
    public string StoreName { get; set; }
    public string OfficeCity { get; set; }
    public string OfficeMetroStation { get; set; }
    public decimal? Weight { get; set; }
    public decimal? ShippingPrice { get; set; }
    public PackageStatus Status { get; set; }
    public string? DeliveryType { get; set; }
    public string? ShipmentType { get; set; }
    public string? Address { get; set; }
    public string? KargomatAddress { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
