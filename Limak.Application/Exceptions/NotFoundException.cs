using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class NotFoundException(string message = "Object is not found") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 404;

}
