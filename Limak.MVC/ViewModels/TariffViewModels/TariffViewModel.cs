using Limak.Application.Dtos.TariffDtos;

namespace Limak.MVC.ViewModels.TariffViewModels;

public class TariffViewModel
{
    public List<TariffGetDto> Tariffs { get; set; } = new List<TariffGetDto>();
}
