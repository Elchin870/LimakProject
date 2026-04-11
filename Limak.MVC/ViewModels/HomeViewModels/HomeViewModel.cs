using Limak.Application.Dtos.AccountDtos;
using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.PartnerDtos;
using Limak.Application.Dtos.ShopDtos;
using Limak.Application.Dtos.TariffDtos;

namespace Limak.MVC.ViewModels.HomeViewModels;

public class HomeViewModel
{
    public RegisterDto Register { get; set; } = new RegisterDto();
    public List<OfficeGetDto> Offices { get; set; } = new List<OfficeGetDto>();
    public List<ShopGetDto> Shops { get; set; } = new List<ShopGetDto>();
    public List<AnnouncementGetDto> Announcements { get; set; } = new List<AnnouncementGetDto>();
    public List<TariffGetDto> Tariffs { get; set; } = new List<TariffGetDto>();
    public List<PartnerGetDto> Partners { get; set; } = new List<PartnerGetDto>();
}
