using Limak.Application.Dtos.CustomerDtos;

namespace Limak.MVC.ViewModels.SettingsViewModels;

public class ChangePasswordViewModel
{
    public ChangePasswordDto Password { get; set; } = new();
}
