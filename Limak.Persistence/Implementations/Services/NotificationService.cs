using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.NotificationDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(InternalNotificationCreateDto dto)
    {
        var notification = new Notification
        {
            CustomerId = dto.CustomerId,
            Type = dto.Type,
            Title = dto.Title,
            Message = dto.Message,
            OrderId = dto.OrderId,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ResultDto> CreateNotificationAsync(NotificationCreateDto dto)
    {
        var notification = new Notification
        {
            CustomerId = dto.CustomerId,
            Type = dto.Type,
            Title = dto.Title,
            Message = dto.Message,
            OrderId = dto.OrderId,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        return new("Notification created successfully");
    }

    public async Task<ResultDto<List<AdminNotificationGetDto>>> GetAllAsync()
    {
        var notifications = await _notificationRepository
        .GetAll()
        .Include(x => x.Customer)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();

        var result = notifications.Select(x => new AdminNotificationGetDto
        {
            Id = x.Id,
            CustomerFirstName = x.Customer.AppUser.FirstName,
            CustomerLastName = x.Customer.AppUser.LastName,
            CustomerCode = x.Customer.CustomerCode,
            Type = x.Type,
            Title = x.Title,
            Message = x.Message,
            OrderId = x.OrderId,
            IsRead = x.IsRead,
            ReadAt = x.ReadAt,
            CreatedAt = x.CreatedAt
        }).ToList();

        return new(result);
    }

    public async Task<ResultDto<List<UserNotificationGetDto>>> GetUserNotificationsAsync(Guid customerId)
    {
        var notifications = await _notificationRepository
        .GetAll()
        .Where(x => x.CustomerId == customerId)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();

        var result = notifications.Select(x => new UserNotificationGetDto
        {
            Id = x.Id,
            Type = x.Type,
            Title = x.Title,
            Message = x.Message,
            OrderId = x.OrderId,
            IsRead = x.IsRead,
            ReadAt = x.ReadAt,
            CreatedAt = x.CreatedAt
        }).ToList();

        return new(result);
    }

    public async Task<ResultDto> MarkAllAsReadAsync(Guid customerId)
    {
        var notifications = await _notificationRepository
            .GetAll()
            .Where(x => x.CustomerId == customerId && !x.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            _notificationRepository.Update(notification);
        }

        await _unitOfWork.SaveChangesAsync();

        return new($"{notifications.Count} notifications marked as read");
    }

    public async Task<ResultDto> MarkAsReadAsync(Guid customerId, Guid notificationId)
    {
        var notification = await _notificationRepository.GetAsync(x => x.Id == notificationId && x.CustomerId == customerId);
        if (notification == null)
        {
            throw new NotFoundException("Notification not found");
        }

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            _notificationRepository.Update(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        return new ResultDto("Notification readed");
    }
}
