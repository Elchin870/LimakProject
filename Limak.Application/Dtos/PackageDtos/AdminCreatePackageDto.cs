namespace Limak.Application.Dtos.PackageDtos;

public class AdminCreatePackageDto
{
    public string CustomerCode { get; set; }
    public string StoreName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Guid CountryId { get; set; }
}
