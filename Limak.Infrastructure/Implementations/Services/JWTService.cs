using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.TokenDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Limak.Infrastructure.Implementations.Services;

public class JWTService : IJWTService
{
    private readonly JWTOptionsDto _jwTOptionsDto;
    public JWTService(IConfiguration configuration)
    {
        _jwTOptionsDto = configuration.GetSection("JWTOptions").Get<JWTOptionsDto>() ?? new();
    }
    public AccessTokenDto CreateAccessToken(List<Claim> claims)
    {
        string secrecKey = _jwTOptionsDto.SecretKey;

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrecKey));

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtHeader jwtHeader = new JwtHeader(signingCredentials);

        JwtPayload payload = new JwtPayload(_jwTOptionsDto.Issuer, _jwTOptionsDto.Audience, claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(60));

        JwtSecurityToken securityToken = new JwtSecurityToken(jwtHeader, payload);

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        string token = handler.WriteToken(securityToken);

        string refreshToken = Guid.NewGuid().ToString();


        return new()
        {
            Token = token,
            ExpireDate = DateTime.UtcNow.AddMinutes(_jwTOptionsDto.ExpiredDate),
            RefreshToken = refreshToken,
            RefreshTokenExpiredDate = DateTime.UtcNow.AddMinutes(_jwTOptionsDto.ExpiredDate + 60),
        };
    }
}
