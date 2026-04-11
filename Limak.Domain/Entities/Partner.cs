using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Partner : BaseAuditableEntity
{
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public string WebsitePath { get; set; }
}
