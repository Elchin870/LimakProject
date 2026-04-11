using Limak.Application.Dtos.CountryAddressDtos;
using Limak.Application.Dtos.CustomerDtos;

namespace Limak.MVC.ViewModels.AddressViewModels;

public class AddressViewModel
{
    public CustomerGetDto Customer { get; set; } = new CustomerGetDto();
    public List<CountryAddressGetDto> CountryAddress { get; set; } = new List<CountryAddressGetDto>();
}
