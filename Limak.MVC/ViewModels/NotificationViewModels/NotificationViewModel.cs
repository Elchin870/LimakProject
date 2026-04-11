using Limak.Application.Dtos.NotificationDtos;

namespace Limak.MVC.ViewModels.NotificationViewModels;

public class NotificationViewModel
{
    public List<UserNotificationGetDto> UserNotifications { get; set; } = new List<UserNotificationGetDto>();
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int TotalNotifications { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalNotifications / PageSize);
    public Guid CustomerId { get; set; }
}
