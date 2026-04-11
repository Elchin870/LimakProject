using Limak.Application.Dtos.CalcullatorDtos;
using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Application.Dtos.ShipmentTypeDtos;

namespace Limak.MVC.ViewModels.CalcullatorViewModels;

public class CalculatorViewModel
{
    public ShipmentCalculateDto Calculator { get; set; } = new ShipmentCalculateDto();
    public List<CountryGetDto> Countries { get; set; } = new List<CountryGetDto>();
    public List<ShipmentTypeGetDto> ShipmentTypes { get; set; } = new List<ShipmentTypeGetDto>();
    public List<DeliveryTypeGetDto> DeliveryTypes { get; set; } = new List<DeliveryTypeGetDto>();
    public decimal? TotalPrice { get; set; }
}
