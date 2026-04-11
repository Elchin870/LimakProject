using Microsoft.AspNetCore.Http;

namespace Limak.Application.Dtos.AnnouncementDtos;

public class AnnouncementCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; } = null!;
}
