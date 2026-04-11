namespace Limak.Application.Dtos.KargomatDtos;

public class KargomatUpdateDto
{
    public Guid Id { get; set; }
    public string ShortAddress { get; set; }
    public string FullAddress { get; set; }
    public decimal Price { get; set; }
}