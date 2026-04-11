using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.NotificationDtos;
using Limak.Application.Dtos.ResultDtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Limak.MVC.Controllers;

public class NotificationController(IHttpClientFactory _httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");
    private const int PageSize = 5;
    private const string ApiBaseUrl = "https://localhost:7078/api";
    public async Task<IActionResult> Index(int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var notificationResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Notifications/get-all-notifications/{customerResult.Data!.Id}");
        var notificationResult = await notificationResponse.Content.ReadFromJsonAsync<ResultDto<List<UserNotificationGetDto>>>() ?? new();

        var allNotifications = notificationResult.Data ?? new();
        var totalNotifications = allNotifications.Count;
        
        var paginatedNotifications = allNotifications
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        var viewModel = new ViewModels.NotificationViewModels.NotificationViewModel
        {
            UserNotifications = paginatedNotifications,
            CurrentPage = page,
            PageSize = PageSize,
            TotalNotifications = totalNotifications,
            CustomerId = customerResult.Data!.Id
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var response = await _httpClient.PutAsync($"{ApiBaseUrl}/Notifications/mark-all-as-read/{customerResult.Data!.Id}", null);

        if (response.IsSuccessStatusCode)
        {
            return Json(new { success = true, message = "Bütün bildirişlər oxunmuş kimi işarələndi" });
        }
        
        return Json(new { success = false, message = "Xəta baş verdi" });
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var customer = await _httpClient.GetAsync($"{ApiBaseUrl}/Customers/get-customer/{userId}");
        var customerResult = await customer.Content.ReadFromJsonAsync<ResultDto<CustomerGetDto>>() ?? new();

        var notificationResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/Notifications/get-all-notifications/{customerResult.Data!.Id}");
        var notificationResult = await notificationResponse.Content.ReadFromJsonAsync<ResultDto<List<UserNotificationGetDto>>>() ?? new();

        var allNotifications = notificationResult.Data ?? new();
        var totalNotifications = allNotifications.Count;
        
        var paginatedNotifications = allNotifications
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        var html = new System.Text.StringBuilder();
        
        if (paginatedNotifications.Any())
        {
            html.Append("<div class=\"notifications-list\">");
            foreach (var userNotification in paginatedNotifications)
            {
                var isRead = userNotification.IsRead;
                var fontWeight = isRead ? "" : "font-weight: 700;";
                var bgColor = isRead ? "#f8f9fa" : "#fff9e6";
                var textColor = isRead ? "color: #6c757d;" : "color: #212529;";
                var messageColor = isRead ? "color: #6c757d;" : "color: #495057;";
                var borderColor = GetNotificationColor(userNotification.Type);
                
                html.Append($@"
                    <div class='notification-item {(isRead ? "read" : "unread")}' 
                         style='{fontWeight}padding: 2rem;border-left: 5px solid {borderColor};margin-bottom: 1.5rem;background: {bgColor};border-radius: 0.75rem;box-shadow: 0 2px 6px rgba(0,0,0,0.08);transition: all 0.3s ease;min-height: 140px;'>
                        <div style='display: flex; justify-content: space-between; align-items: flex-start; height: 100%;'>
                            <div style='flex: 1;'>
                                <div style='display: flex; align-items: center; gap: 1rem; margin-bottom: 0.75rem;'>
                                    <span class='badge badge-{GetNotificationBadgeClass(userNotification.Type)}' 
                                          style='padding: 0.5rem 1rem; font-size: 0.8rem; text-transform: uppercase; font-weight: 600;'>
                                        {userNotification.Type}
                                    </span>
                                    {(isRead ? "" : "<span style='width: 10px; height: 10px; background: #007bff; border-radius: 50%; display: inline-block; flex-shrink: 0;'></span>")}
                                </div>
                                <h4 style='margin: 0.75rem 0; {textColor}; font-size: 1.1rem; font-weight: 700;'>
                                    {userNotification.Title}
                                </h4>
                                <p style='margin: 1rem 0 0 0; {messageColor}; font-size: 0.95rem; line-height: 1.5;'>
                                    {userNotification.Message}
                                </p>
                                <small style='display: block; margin-top: 1rem; color: #999; font-size: 0.85rem;'>
                                    <i class='fas fa-calendar'></i> {userNotification.CreatedAt.ToString("dd.MM.yyyy HH:mm")}
                                </small>
                            </div>
                            <div style='margin-left: 1.5rem; display: flex; flex-direction: column; align-items: flex-end; justify-content: center;'>
                                {(isRead ? 
                                    "<small style='color: #999; white-space: nowrap; font-size: 0.9rem;'><i class='fas fa-check-double' style='color: #28a745; margin-right: 0.5rem;'></i> Oxundu</small>" :
                                    $"<button class='btn btn-sm btn-primary mark-as-read-btn' data-id='{userNotification.Id}' data-customer-id='{customerResult.Data!.Id}' style='white-space: nowrap; padding: 0.6rem 1.2rem; font-weight: 600; border-radius: 0.4rem;'>Oxu</button>"
                                )}
                            </div>
                        </div>
                    </div>
                ");
            }
            html.Append("</div>");
            
            var totalPages = (int)Math.Ceiling((double)totalNotifications / PageSize);
            if (totalPages > 1)
            {
                html.Append($@"
                    <nav style='margin-top: 2rem;'>
                        <ul class='pagination pagination-lg' style='justify-content: center;'>
                ");
                
                if (page > 1)
                {
                    html.Append($@"
                        <li class='page-item'>
                            <a class='page-link pagination-btn' href='#' data-page='{page - 1}' style='cursor: pointer;'>
                                <i class='fas fa-chevron-left'></i> Əvvəl
                            </a>
                        </li>
                    ");
                }
                
                int startPage = Math.Max(1, page - 2);
                int endPage = Math.Min(totalPages, page + 2);
                
                if (startPage > 1)
                {
                    html.Append($@"
                        <li class='page-item'>
                            <a class='page-link pagination-btn' href='#' data-page='1' style='cursor: pointer;'>1</a>
                        </li>
                    ");
                    if (startPage > 2)
                    {
                        html.Append("<li class='page-item disabled'><span class='page-link'>...</span></li>");
                    }
                }
                
                for (int i = startPage; i <= endPage; i++)
                {
                    if (i == page)
                    {
                        html.Append($@"
                            <li class='page-item active' aria-current='page'>
                                <span class='page-link'>{i}<span class='sr-only'>(current)</span></span>
                            </li>
                        ");
                    }
                    else
                    {
                        html.Append($@"
                            <li class='page-item'>
                                <a class='page-link pagination-btn' href='#' data-page='{i}' style='cursor: pointer;'>{i}</a>
                            </li>
                        ");
                    }
                }
                
                if (endPage < totalPages)
                {
                    if (endPage < totalPages - 1)
                    {
                        html.Append("<li class='page-item disabled'><span class='page-link'>...</span></li>");
                    }
                    html.Append($@"
                        <li class='page-item'>
                            <a class='page-link pagination-btn' href='#' data-page='{totalPages}' style='cursor: pointer;'>{totalPages}</a>
                        </li>
                    ");
                }
                
                if (page < totalPages)
                {
                    html.Append($@"
                        <li class='page-item'>
                            <a class='page-link pagination-btn' href='#' data-page='{page + 1}' style='cursor: pointer;'>
                                Sonrakı <i class='fas fa-chevron-right'></i>
                            </a>
                        </li>
                    ");
                }
                
                html.Append("</ul></nav>");
            }
        }
        else
        {
            html.Append(@"
                <div style='text-align: center; padding: 4rem 1rem;'>
                    <i class='fas fa-bell' style='font-size: 4rem; color: #ddd; margin-bottom: 1.5rem; display: block;'></i>
                    <p style='color: #999; font-size: 1.2rem;'>Heç bir bildirişiniz yoxdur</p>
                </div>
            ");
        }
        
        return Content(html.ToString(), "text/html");
    }

    private string GetNotificationColor(object type)
    {
        return type?.ToString().ToLower() switch
        {
            "info" => "#17a2b8",
            "success" => "#28a745",
            "warning" => "#ffc107",
            "error" => "#dc3545",
            _ => "#6c757d"
        };
    }

    private string GetNotificationBadgeClass(object type)
    {
        return type?.ToString().ToLower() switch
        {
            "info" => "info",
            "success" => "success",
            "warning" => "warning",
            "error" => "error",
            _ => "secondary"
        };
    }
}
