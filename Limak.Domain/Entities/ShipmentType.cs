using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class ShipmentType : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}
