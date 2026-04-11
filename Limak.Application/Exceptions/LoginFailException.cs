using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class LoginFailException(string message = "Login failed") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 400;
}
