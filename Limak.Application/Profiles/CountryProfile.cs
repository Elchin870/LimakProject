using AutoMapper;
using Limak.Application.Dtos.CountryDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class CountryProfile : Profile
{
    public CountryProfile()
    {
        CreateMap<Country, CountryGetDto>().ReverseMap();
        CreateMap<Country, CountryCreateDto>().ReverseMap();
        CreateMap<Country, CountryUpdateDto>().ReverseMap();
    }
}
