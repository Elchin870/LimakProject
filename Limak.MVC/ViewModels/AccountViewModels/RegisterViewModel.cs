using Limak.Application.Dtos.AccountDtos;
using Limak.Application.Dtos.OfficeDtos;

namespace Limak.MVC.ViewModels.AccountViewModels;

public class RegisterViewModel
{
    public RegisterDto Register { get; set; } = new RegisterDto();
    public List<OfficeGetDto> Offices { get; set; } = new List<OfficeGetDto>();
}
