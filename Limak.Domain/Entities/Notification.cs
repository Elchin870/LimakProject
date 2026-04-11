using Limak.Domain.Entities.Common;
using Limak.Domain.Enums;

namespace Limak.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }
}
