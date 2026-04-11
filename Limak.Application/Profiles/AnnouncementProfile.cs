using AutoMapper;
using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class AnnouncementProfile : Profile
{
    public AnnouncementProfile()
    {
        CreateMap<Announcement, AnnouncementGetDto>().ReverseMap();
        CreateMap<Announcement, AnnouncementCreateDto>().ReverseMap();
        CreateMap<Announcement, AnnouncementUpdateDto>().ReverseMap();
    }
}
