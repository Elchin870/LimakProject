using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Tariff : BaseAuditableEntity
{
    public Country Country { get; set; }
    public Guid CountryId { get; set; }
    public DeliveryType DeliveryType { get; set; }
    public Guid DeliveryTypeId { get; set; }
    public ShipmentType ShipmentType { get; set; }
    public Guid ShipmentTypeId { get; set; }
    public decimal MinWeight { get; set; }
    public decimal MaxWeight { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? ExtraPricePerKg { get; set; }
    public decimal BaseWeightLimit { get; set; } = 1;
}
