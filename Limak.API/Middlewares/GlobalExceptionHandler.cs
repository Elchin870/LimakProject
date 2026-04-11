using Limak.Application.Dtos.ResultDtos;

namespace Limak.API.Middlewares;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            ResultDto resultDto = new ResultDto()
            {
                IsSucceed = false,
                Message = ex.Message,
                StatusCode = 500
            };
            await context.Response.WriteAsJsonAsync(resultDto);
        }
    }
}
