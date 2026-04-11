using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Office : BaseAuditableEntity
{
    public string City { get; set; }
    public string? MetroStation { get; set; }
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}
