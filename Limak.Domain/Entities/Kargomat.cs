using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Kargomat : BaseAuditableEntity
{
    public string ShortAddress { get; set; }
    public string FullAddress { get; set; }
    public decimal Price { get; set; }
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}
