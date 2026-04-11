using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
