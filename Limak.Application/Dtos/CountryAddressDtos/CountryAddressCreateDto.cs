namespace Limak.Application.Dtos.CountryAddressDtos;

public class CountryAddressCreateDto
{
    public string City { get; set; }
    public string State { get; set; }
    public Guid CountryId { get; set; }
    public string Address { get; set; }
    public string? TC { get; set; }
}

