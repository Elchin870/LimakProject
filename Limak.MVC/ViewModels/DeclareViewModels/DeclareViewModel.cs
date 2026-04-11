using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Application.Dtos.KargomatDtos;
using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.PackageDtos;
using Limak.Application.Dtos.ShipmentTypeDtos;

namespace Limak.MVC.ViewModels.DeclareViewModels;

public class DeclareViewModel
{
    public List<UserPackageListDto> UserPackages { get; set; } = new List<UserPackageListDto>();
    public List<CountryGetDto> Countries { get; set; } = new List<CountryGetDto>();
    public List<DeliveryTypeGetDto> DeliveryTypes { get; set; } = new List<DeliveryTypeGetDto>();
    public List<ShipmentTypeGetDto> ShipmentTypes { get; set; } = new List<ShipmentTypeGetDto>();
    public List<OfficeGetDto> Offices { get; set; } = new List<OfficeGetDto>();
    public List<KargomatGetDto> Kargomats { get; set; } = new List<KargomatGetDto>();
}
