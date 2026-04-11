using Microsoft.AspNetCore.Http;

namespace Limak.Application.Dtos.AnnouncementDtos;

public class AnnouncementUpdateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
}
