using Limak.Domain.Enums;

namespace Limak.Application.Dtos.NotificationDtos;

public class InternalNotificationCreateDto
{
    public Guid CustomerId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public Guid? OrderId { get; set; }
}
