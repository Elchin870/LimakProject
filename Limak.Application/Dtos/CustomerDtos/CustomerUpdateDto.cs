namespace Limak.Application.Dtos.CustomerDtos;

public class CustomerUpdateDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Guid OfficeId { get; set; }
    public decimal? Balance { get; set; }
}

