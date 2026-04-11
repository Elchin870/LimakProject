namespace Limak.Application.Dtos.CountryDtos;

public class CountryUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal? VolumetricDivisor { get; set; }
}
