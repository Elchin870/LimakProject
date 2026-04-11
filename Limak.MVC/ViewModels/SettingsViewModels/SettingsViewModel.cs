using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.OfficeDtos;

namespace Limak.MVC.ViewModels.SettingsViewModels;

public class SettingsViewModel
{
    public CustomerUpdateDto Customer { get; set; } = new CustomerUpdateDto();
    public List<OfficeGetDto> Offices { get; set; } = new List<OfficeGetDto>();
    public ChangePasswordDto Password { get; set; } = new ChangePasswordDto();
}
