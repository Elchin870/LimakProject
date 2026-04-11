using Limak.Domain.Entities;
using Limak.Domain.Enums;
using Limak.MVC.ViewModels.UserViewModels;
using Limak.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Limak.MVC.ViewComponents;

public class UserAsideViewComponent : ViewComponent
{
    private readonly LimakDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UserAsideViewComponent(LimakDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = UserClaimsPrincipal
            .FindFirstValue(ClaimTypes.NameIdentifier);

        var currentUser = await _userManager.FindByIdAsync(userId);

        if (userId == null)
            return View(null);

        var now = DateTime.UtcNow;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);


        var customer = await _context.Customers
            .Where(c => c.AppUserId == userId)
            .Select(c => new UserAsideVM
            {
                CustomerCode = c.CustomerCode,

                Money = _context.Payments
                .Where(p =>
                    p.AppUserId == userId &&
                    p.PaymentStatus == PaymentStatuses.Fullypaid &&
                    p.CreatedDate >= firstDayOfMonth)
                .Sum(p => (decimal?)p.Amount) ?? 0

            }).FirstOrDefaultAsync();

        if (customer == null)
            return View(null);

        customer.FirstName = currentUser.FirstName;
        customer.LastName = currentUser.LastName;

        return View(customer);
    }
}
