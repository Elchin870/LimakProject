using Microsoft.AspNetCore.Identity;

namespace Limak.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredDate { get; set; }
    public DateTime? LastConfirmationEmailSent { get; set; }
    public Customer? Customer { get; set; }
}
