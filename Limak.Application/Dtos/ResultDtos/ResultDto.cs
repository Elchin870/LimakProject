namespace Limak.Application.Dtos.ResultDtos;

public class ResultDto
{
    public bool IsSucceed { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ResultDto()
    {
        IsSucceed = true;
        StatusCode = 200;
        Message = "Successfully";
    }

    public ResultDto(string message) : this()
    {
        Message = message;
    }

    public ResultDto(string message, int statusCode) : this(message)
    {
        StatusCode = statusCode;
    }

    public ResultDto(string message, int statusCode, bool isSucceed) : this(message, statusCode)
    {
        IsSucceed = isSucceed;
    }

}

public class ResultDto<T> : ResultDto
{
    public T? Data { get; set; }

    public ResultDto() : base()
    {

    }

    public ResultDto(T data) : this()
    {
        Data = data;
    }

    public ResultDto(string message, T data) : base(message)
    {
        Data = data;
    }

    public ResultDto(string message, T data, int statusCode) : base(message, statusCode)
    {
        Data = data;
    }

    public ResultDto(string message, T data, int statusCode, bool isSucceed) : base(message, statusCode, isSucceed)
    {
        Data = data;
    }
}
