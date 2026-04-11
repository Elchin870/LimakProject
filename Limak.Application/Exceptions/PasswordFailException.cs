using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class PasswordFailException(string message = "Password change failed") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 400;
}
