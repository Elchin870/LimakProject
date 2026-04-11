using Limak.Application.Dtos.AccountDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.TokenDtos;
using Limak.Domain.Entities;

namespace Limak.Application.Abstractions.Services;

public interface IAuthService
{
    Task<ResultDto> Register(RegisterDto dto);
    Task<ResultDto<AccessTokenDto>> Login(LoginDto dto);
    Task<ResultDto> ConfirmEmailAsync(string userId, string token);
    Task<ResultDto<AccessTokenDto>> RefreshToken(string token);
    Task<ResultDto> RevokeRefreshToken(string refreshToken);
}
