namespace Limak.Application.Dtos.PackageDtos;

public class UserPackageDeclareDto
{
    public Guid CustomerId { get; set; }
    public Guid CountryId { get; set; }
    public Guid PackageId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Guid DeliveryTypeId { get; set; }
    public Guid ShipmentTypeId { get; set; }
    public Guid? OfficeId { get; set; }
    public Guid? KargomatId { get; set; }
    public string? Address { get; set; }
}
