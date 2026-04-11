using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; }
    public decimal VolumetricDivisor { get; set; } = 6000;
    public ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
    public ICollection<CountryAddress> CountryAddresses { get; set; } = new List<CountryAddress>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}
