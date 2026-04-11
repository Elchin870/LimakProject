using AutoMapper;
using Limak.Application.Dtos.OfficeDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class OfficeProfile : Profile
{
    public OfficeProfile()
    {
        CreateMap<OfficeCreateDto, Office>().ReverseMap();
        CreateMap<OfficeGetDto, Office>().ReverseMap();
        CreateMap<OfficeUpdateDto, Office>().ReverseMap();
    }
}
