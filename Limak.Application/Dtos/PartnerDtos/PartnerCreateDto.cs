using Microsoft.AspNetCore.Http;

namespace Limak.Application.Dtos.PartnerDtos;

public class PartnerCreateDto
{
    public string Name { get; set; }
    public string WebsitePath { get; set; }
    public IFormFile Image { get; set; } = null!;
}
