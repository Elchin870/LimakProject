using Microsoft.AspNetCore.Http;

namespace Limak.Application.Dtos.ShopDtos;

public class ShopCreateDto
{
    public string Name { get; set; }
    public IFormFile Image { get; set; } = null!;
    public string WebsitePath { get; set; }
    public Guid CategoryId { get; set; }
}
