using Limak.Application.Dtos.CategoryDtos;
using Limak.Application.Dtos.ShopDtos;

namespace Limak.MVC.ViewModels.ShopViewModels;

public class ShopViewModel
{
    public List<ShopGetDto> Shops { get; set; } = new List<ShopGetDto>();
}
