using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class RegisterFailException(string message = "Register failed") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 400;
}
