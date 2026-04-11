using Limak.Application.Dtos.OrderDtos;
using Limak.Domain.Enums;

namespace Limak.MVC.ViewModels.UserPanelViewModels;

public class UserPanelViewModel
{
    public List<UserOrderGetDto> UserOrders { get; set; } = new List<UserOrderGetDto>();
    public List<CountryFilterItem> Countries { get; set; } = new List<CountryFilterItem>();
    
    // Filtering & Pagination
    public Guid? SelectedCountryId { get; set; }
    public OrderStatus? SelectedStatus { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int TotalPages { get; set; }
    public int TotalOrders { get; set; }
}

public class CountryFilterItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
