using Limak.Application.Dtos.PackageDtos;

namespace Limak.MVC.ViewModels.PackageViewModels;

public class AdminPackageViewModel
{
    public List<AdminPackageListDto> AdminPackageLists { get; set; } = new List<AdminPackageListDto>();
}
