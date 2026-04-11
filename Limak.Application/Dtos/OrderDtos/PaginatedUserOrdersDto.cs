namespace Limak.Application.Dtos.OrderDtos;

public class PaginatedUserOrdersDto
{
    public List<UserOrderGetDto> Orders { get; set; } = new List<UserOrderGetDto>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
