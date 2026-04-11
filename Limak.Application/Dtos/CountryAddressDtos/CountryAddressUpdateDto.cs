namespace Limak.Application.Dtos.CountryAddressDtos;

public class CountryAddressUpdateDto
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public Guid CountryId { get; set; }
    public string Address { get; set; }
    public string? TC { get; set; }
}

