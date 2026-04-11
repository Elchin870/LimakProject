using Limak.Application.Dtos.TokenDtos;
using System.Security.Claims;

namespace Limak.Application.Abstractions.Services;

public interface IJWTService
{
    AccessTokenDto CreateAccessToken(List<Claim> claims);
}
