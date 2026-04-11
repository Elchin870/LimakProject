using Limak.Application.Dtos.PackageDtos;

namespace Limak.MVC.ViewModels.DeclareViewModels;

public class DeclareViewModelAdmin
{
    public List<AdminPackageListDto> Packages { get; set; } = new List<AdminPackageListDto>();
}
