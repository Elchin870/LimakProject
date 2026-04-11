using AutoMapper;
using Limak.Application.Dtos.PaymentDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Profiles;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentCreateDto>().ReverseMap();
        CreateMap<Payment, PaymentGetDto>().ReverseMap();
        CreateMap<Payment, PaymentUpdateDto>().ReverseMap();
    }
}
