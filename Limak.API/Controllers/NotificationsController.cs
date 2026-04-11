using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.NotificationDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Limak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private Guid GetCustomerId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet("get-all-notifications/{id}")]
        public async Task<IActionResult> GetAllNotifications(Guid id)
        {
            var result = await _notificationService.GetUserNotificationsAsync(id);
            return Ok(result);
        }

        [HttpPut("mark-as-read/{id}/{customerId}")]
        public async Task<IActionResult> MarkAsRead(Guid id, Guid customerId)
        {
            var result = await _notificationService.MarkAsReadAsync(customerId, id);
            return Ok(result);
        }

        [HttpGet("get-notifications-admin")]
        public async Task<IActionResult> GetNotificationsAdmin()
        {
            var result = await _notificationService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("create-notification")]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateDto dto)
        {
            var result = await _notificationService.CreateNotificationAsync(dto);
            return Ok(result);
        }

        [HttpPut("mark-all-as-read/{id}")]
        public async Task<IActionResult> MarkAllAsRead(Guid id)
        {
            var result = await _notificationService.MarkAllAsReadAsync(id);
            return Ok(result);
        }
    }
}
