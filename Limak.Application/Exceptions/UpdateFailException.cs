using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class UpdateFailException(string message = "Update failed") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 400;
}