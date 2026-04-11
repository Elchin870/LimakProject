using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.BalanceTransactionDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class BalanceTransactionService : IBalanceTransactionService
{
    private readonly IBalanceTransactionRepository _balanceTransactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public BalanceTransactionService(IBalanceTransactionRepository balanceTransactionRepository, IUnitOfWork unitOfWork)
    {
        _balanceTransactionRepository = balanceTransactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(InternalBalanceTransactionCreateDto dto)
    {
        var transaction = new BalanceTransaction
        {
            CustomerId = dto.CustomerId,
            Type = dto.Type,
            Amount = dto.Amount,
            Description = dto.Description,
            OrderId = dto.OrderId
        };

        await _balanceTransactionRepository.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ResultDto> CreateAsync(BalanceTransactionCreateDto dto)
    {
        var transaction = new BalanceTransaction
        {
            CustomerId = dto.CustomerId,
            Type = dto.Type,
            Amount = dto.Amount,
            Description = dto.Description,
            OrderId = dto.OrderId,
            CreatedAt = dto.CreatedAt
        };

        await _balanceTransactionRepository.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return new("Transaction created successfully");
    }

    public async Task<ResultDto<List<AdminBalanceTransactionGetDto>>> GetAllAsync()
    {
        var transactions = await _balanceTransactionRepository
        .GetAll()
        .Include(x => x.Customer)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();

        var result = transactions.Select(x => new AdminBalanceTransactionGetDto
        {
            Id = x.Id,
            CustomerFirstName = x.Customer.AppUser.FirstName,
            CustomerLastName = x.Customer.AppUser.LastName,
            CustomerCode = x.Customer.CustomerCode,
            Type = x.Type,
            Amount = x.Amount,
            Description = x.Description,
            OrderId = x.OrderId,
            CreatedAt = x.CreatedAt
        }).ToList();

        return new(result);
    }

    public async Task<ResultDto<List<AdminBalanceTransactionGetDto>>> GetByCustomerAsync(Guid customerId)
    {
        var transactions = await _balanceTransactionRepository
        .GetAll()
        .Where(x => x.CustomerId == customerId)
        .Include(x => x.Customer)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();

        var result = transactions.Select(x => new AdminBalanceTransactionGetDto
        {
            Id = x.Id,
            CustomerFirstName = x.Customer.AppUser.FirstName,
            CustomerLastName = x.Customer.AppUser.LastName,
            CustomerCode = x.Customer.CustomerCode,
            Type = x.Type,
            Amount = x.Amount,
            Description = x.Description,
            OrderId = x.OrderId,
            CreatedAt = x.CreatedAt
        }).ToList();

        return new(result);
    }

    public async Task<ResultDto<List<BalanceTransactionGetDto>>> GetByCustomerIdAsync(Guid customerId)
    {
        var transactions = await _balanceTransactionRepository.GetAll()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new BalanceTransactionGetDto
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Type = x.Type,
                Amount = x.Amount,
                Description = x.Description,
                OrderId = x.OrderId,
                CreatedAt = x.CreatedAt
            }).ToListAsync();

        return new(transactions);
    }
}
