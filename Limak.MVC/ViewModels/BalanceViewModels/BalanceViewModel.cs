using Limak.Application.Dtos.BalanceTransactionDtos;
using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.PaymentDtos;
using Limak.Domain.Enums;

namespace Limak.MVC.ViewModels.BalanceViewModels;

public class BalanceViewModel
{
    public CustomerGetDto Customer { get; set; } = new CustomerGetDto();
    public PaymentGetDto Payment { get; set; } = new PaymentGetDto();
    public List<BalanceTransactionGetDto> BalanceTransactions { get; set; } = new List<BalanceTransactionGetDto>();
    
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }
    public int TotalPages => (TotalItems + PageSize - 1) / PageSize;
    
    public int FilterType { get; set; } = 0; 
}
