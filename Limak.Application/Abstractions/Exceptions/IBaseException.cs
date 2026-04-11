namespace Limak.Application.Abstractions.Exceptions;

public interface IBaseException
{
    public int StatusCode { get; set; }
}
