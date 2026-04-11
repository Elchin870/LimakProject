using Limak.Domain.Enums;

namespace Limak.Application.Dtos.PackageDtos;

public class UserPackageListDto
{
    public Guid Id { get; set; }
    public string CountryName { get; set; }
    public string? OrderNumber { get; set; }
    public string TrackingNumber { get; set; }
    public string StoreName { get; set; }
    public decimal? Weight { get; set; }
    public decimal? ShippingPrice { get; set; }
    public decimal OrderPrice { get; set; }
    public PackageStatus Status { get; set; }
    public string? AdminNote { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }
}
