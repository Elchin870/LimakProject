using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.OfficeDtos;

namespace Limak.MVC.ViewModels.SettingsViewModels;

public class UpdateCustomerViewModel
{
    public CustomerUpdateDto Customer { get; set; } = null!;
    public List<OfficeGetDto> Offices { get; set; } = new();
}
