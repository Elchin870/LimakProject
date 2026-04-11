using AutoMapper;
using Limak.Application.Dtos.CountryAddressDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class CountryAddressProfile : Profile
{
    public CountryAddressProfile()
    {
        CreateMap<CountryAddress, CountryAddressGetDto>().ReverseMap();
        CreateMap<CountryAddress, CountryAddressCreateDto>().ReverseMap();
        CreateMap<CountryAddress, CountryAddressUpdateDto>().ReverseMap();
    }
}
