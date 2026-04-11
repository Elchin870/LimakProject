using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class AlreadyExistException(string message = "This item is already exist") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 409;
}
