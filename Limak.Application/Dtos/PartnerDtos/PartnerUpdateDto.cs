using Microsoft.AspNetCore.Http;

namespace Limak.Application.Dtos.PartnerDtos;

public class PartnerUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string WebsitePath { get; set; }
    public IFormFile? Image { get; set; }
}