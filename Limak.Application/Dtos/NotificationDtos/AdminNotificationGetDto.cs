using Limak.Domain.Enums;

namespace Limak.Application.Dtos.NotificationDtos;

public class AdminNotificationGetDto
{
    public Guid Id { get; set; }

    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string CustomerCode { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }

    public Guid? OrderId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
