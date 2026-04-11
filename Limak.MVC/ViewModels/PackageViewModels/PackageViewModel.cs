using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.PackageDtos;

namespace Limak.MVC.ViewModels.PackageViewModels;

public class PackageViewModel
{
    public List<UserPackageListDto> UserPackages { get; set; } = new List<UserPackageListDto>();
    public List<CountryGetDto> Countries { get; set; } = new List<CountryGetDto>();
}
