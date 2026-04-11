using AutoMapper;
using Limak.Application.Dtos.TariffDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class TariffProfile : Profile
{
    public TariffProfile()
    {
        CreateMap<Tariff, TariffGetDto>()
                            .ForMember(x => x.CountryName,
                                opt => opt.MapFrom(x => x.Country.Name))
                            .ForMember(x => x.DeliveryTypeName,
                                opt => opt.MapFrom(x => x.DeliveryType.Name))
                            .ForMember(x => x.ShipmentTypeName,
                                opt => opt.MapFrom(x => x.ShipmentType.Name));
        CreateMap<Tariff, TariffCreateDto>().ReverseMap();
        CreateMap<Tariff, TariffUpdateDto>().ReverseMap();
    }
}
