using Limak.Application.Dtos.AccountDtos;
using Limak.Domain.Entities;
using Limak.Domain.Enums;
using Limak.Persistence.Abstractions;
using Limak.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Limak.Persistence.ContextInitalizers;

internal class DbContextInitalizer : IContextInitalizer
{
    private readonly LimakDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AdminDto _adminDto;

    public DbContextInitalizer(LimakDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _adminDto = _configuration.GetRequiredSection("AdminSettings").Get<AdminDto>() ?? new();
    }

    public async Task InitDatabaseAsync()
    {
        await _context.Database.MigrateAsync();
        await CreateRoles();
        await CreateAdmin();
    }

    private async Task CreateAdmin()
    {
        AppUser adminUser = new AppUser()
        {
            UserName = _adminDto.UserName,
            FirstName = _adminDto.FirstName,
            LastName = _adminDto.LastName,
            Email = _adminDto.Email,
            RefreshToken = "",
            RefreshTokenExpiredDate = DateTime.UtcNow,
        };

        var result = await _userManager.CreateAsync(adminUser, _adminDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(adminUser, IdentityRoles.Admin.ToString());
        }
    }

    private async Task CreateRoles()
    {
        foreach (var role in Enum.GetNames(typeof(IdentityRoles)))
        {
            await _roleManager.CreateAsync(new()
            {
                Name = role
            });
        }
    }
}
