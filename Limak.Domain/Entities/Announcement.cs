using Limak.Domain.Entities.Common;

namespace Limak.Domain.Entities;

public class Announcement : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
}
