using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class EmailConfirmationFailException(string message = "Email confirmation failed") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 400;
}
