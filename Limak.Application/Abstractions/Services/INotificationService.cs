using Limak.Application.Dtos.NotificationDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface INotificationService
{
    Task<ResultDto<List<UserNotificationGetDto>>> GetUserNotificationsAsync(Guid customerId);

    Task<ResultDto> MarkAsReadAsync(Guid customerId, Guid notificationId);

    Task<ResultDto<List<AdminNotificationGetDto>>> GetAllAsync();

    Task CreateAsync(InternalNotificationCreateDto dto);
    Task<ResultDto> MarkAllAsReadAsync(Guid customerId);
    Task<ResultDto> CreateNotificationAsync(NotificationCreateDto dto);
}
