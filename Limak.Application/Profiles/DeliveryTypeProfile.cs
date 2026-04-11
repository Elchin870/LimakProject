using AutoMapper;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class DeliveryTypeProfile : Profile
{
    public DeliveryTypeProfile()
    {
        CreateMap<DeliveryType, DeliveryTypeGetDto>().ReverseMap();
        CreateMap<DeliveryType, DeliveryTypeCreateDto>().ReverseMap();
        CreateMap<DeliveryType, DeliveryTypeUpdateDto>().ReverseMap();
    }
}
