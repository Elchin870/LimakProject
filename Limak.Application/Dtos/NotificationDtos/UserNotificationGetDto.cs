using Limak.Domain.Enums;

namespace Limak.Application.Dtos.NotificationDtos;

public class UserNotificationGetDto
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public Guid? OrderId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
