namespace Limak.Application.Dtos.TariffDtos;

public class TariffUpdateDto
{
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }
    public Guid DeliveryTypeId { get; set; }
    public Guid ShipmentTypeId { get; set; }

    public decimal MinWeight { get; set; }
    public decimal MaxWeight { get; set; }

    public decimal BasePrice { get; set; }
    public decimal? ExtraPricePerKg { get; set; }
    public decimal BaseWeightLimit { get; set; }
}
