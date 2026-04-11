namespace Limak.Application.Dtos.TariffDtos;

public class TariffGetDto
{
    public Guid Id { get; set; }

    public string CountryName { get; set; }
    public string DeliveryTypeName { get; set; }
    public string ShipmentTypeName { get; set; }

    public decimal MinWeight { get; set; }
    public decimal MaxWeight { get; set; }

    public decimal BasePrice { get; set; }
    public decimal? ExtraPricePerKg { get; set; }
    public decimal BaseWeightLimit { get; set; }
}
