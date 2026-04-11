namespace Limak.Application.Dtos.TokenDtos;

public class JWTOptionsDto
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SecretKey { get; set; }
    public int ExpiredDate { get; set; }
}
