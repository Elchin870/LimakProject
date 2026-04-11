namespace Limak.Application.Dtos.CustomerDtos;

public class ChangePasswordDto
{
    public Guid CustomerId { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}
