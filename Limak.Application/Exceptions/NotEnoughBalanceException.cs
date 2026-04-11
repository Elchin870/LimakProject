using Limak.Application.Abstractions.Exceptions;

namespace Limak.Application.Exceptions;

public class NotEnoughBalanceException(string message = "Not enough balance") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 400;
}
