using Microsoft.AspNetCore.Http;

namespace Limak.Application.Dtos.ShopDtos;

public class ShopUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
    public string WebsitePath { get; set; }
    public Guid CategoryId { get; set; }
}
