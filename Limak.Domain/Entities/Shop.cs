using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Shop : BaseAuditableEntity
{
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public string WebsitePath { get; set; }
    public Category Category { get; set; }
    public Guid CategoryId { get; set; }
}
