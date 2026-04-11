using AutoMapper;
using Limak.Application.Dtos.ShipmentTypeDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class ShipmentTypeProfile : Profile
{
    public ShipmentTypeProfile()
    {
        CreateMap<ShipmentType, ShipmentTypeGetDto>().ReverseMap();
        CreateMap<ShipmentType, ShipmentTypeCreateDto>().ReverseMap();
        CreateMap<ShipmentType, ShipmentTypeUpdateDto>().ReverseMap();
    }
}
