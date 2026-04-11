using AutoMapper;
using Limak.Application.Dtos.AccountDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, RegisterDto>().ReverseMap();
    }
}
