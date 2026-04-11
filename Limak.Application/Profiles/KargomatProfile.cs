using AutoMapper;
using Limak.Application.Dtos.KargomatDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class KargomatProfile : Profile
{
    public KargomatProfile()
    {
        CreateMap<Kargomat, KargomatGetDto>().ReverseMap();
        CreateMap<Kargomat, KargomatCreateDto>().ReverseMap();
        CreateMap<Kargomat, KargomatUpdateDto>().ReverseMap();
    }
}
