namespace Limak.Application.Dtos.TokenDtos;

public class AccessTokenDto
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiredDate { get; set; }
}
