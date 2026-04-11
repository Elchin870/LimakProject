using AutoMapper;
using Limak.Application.Dtos.PartnerDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class PartnerProfile : Profile
{
    public PartnerProfile()
    {
        CreateMap<Partner, PartnerGetDto>().ReverseMap();
        CreateMap<Partner, PartnerCreateDto>().ReverseMap();
        CreateMap<Partner, PartnerUpdateDto>().ReverseMap();
    }
}
