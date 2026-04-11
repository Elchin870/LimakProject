namespace Limak.Application.Dtos.CalcullatorDtos;

public class ShipmentCalculateDto
{
    public Guid CountryId { get; set; }
    public Guid DeliveryTypeId { get; set; }
    public Guid ShipmentTypeId { get; set; }
    public decimal Weight { get; set; }
    public decimal? Width { get; set; }
    public decimal? Length { get; set; }
    public decimal? Height { get; set; }
}
