using AutoMapper;
using Limak.Application.Dtos.ShopDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<Shop, ShopGetDto>().ReverseMap();
        CreateMap<Shop, ShopCreateDto>().ReverseMap();
        CreateMap<Shop, ShopUpdateDto>().ReverseMap();
    }
}
