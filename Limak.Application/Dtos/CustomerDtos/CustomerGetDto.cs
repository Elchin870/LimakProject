namespace Limak.Application.Dtos.CustomerDtos;

public class CustomerGetDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string OfficeCity { get; set; }
    public string OfficeMetroStation { get; set; }
    public string CustomerCode { get; set; }
    public decimal Balance { get; set; }
}

