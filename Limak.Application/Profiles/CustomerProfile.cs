using AutoMapper;
using Limak.Application.Dtos.CustomerDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerGetDto>()
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.AppUser.FirstName))
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.AppUser.LastName))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.AppUser.Email))
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.AppUser.UserName))
            .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.AppUser.PhoneNumber))
            .ForMember(x => x.OfficeCity, opt => opt.MapFrom(x => x.Office.City))
            .ForMember(x => x.OfficeMetroStation, opt => opt.MapFrom(x => x.Office.MetroStation));

        CreateMap<Customer, CustomerUpdateDto>()
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.AppUser.FirstName))
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.AppUser.LastName))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.AppUser.Email))
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.AppUser.UserName))
            .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.AppUser.PhoneNumber))
            .ForMember(x => x.OfficeId, opt => opt.MapFrom(x => x.Office.Id));

    }
}
