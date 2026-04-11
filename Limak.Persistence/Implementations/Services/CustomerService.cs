using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CustomerDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Limak.Persistence.Implementations.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    public CustomerService(ICustomerRepository repository, IMapper mapper, UserManager<AppUser> userManager)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ResultDto> ChangePassword(ChangePasswordDto dto)
    {
        var customer = await _repository.GetByIdAsync(dto.CustomerId);
        if (customer == null)
        {
            throw new NotFoundException("Customer is not found");
        }

        var user = await _userManager.FindByIdAsync(customer.AppUserId);
        if (user == null)
        {
            throw new NotFoundException("User is not found");
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            throw new PasswordFailException("Password is wrong please try again");
        }

        return new("Password change successfully");
    }

    public async Task<ResultDto<CustomerGetDto>> GetAsync(string id)
    {
        var customer = await _repository.GetAsync(x => x.AppUserId == id, x => x.AppUser, x => x.Office);
        if (customer == null)
        {
            throw new NotFoundException("Customer is not found");
        }
        var dto = _mapper.Map<CustomerGetDto>(customer);
        return new(dto);
    }

    public async Task<ResultDto<CustomerGetDto>> GetByIdAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new NotFoundException("Customer is not found");
        }
        var dto = _mapper.Map<CustomerGetDto>(customer);
        return new(dto);
    }

    public async Task<ResultDto<CustomerUpdateDto>> GetUpdateDto(string appUserId)
    {
        var customer = await _repository.GetAsync(x => x.AppUserId == appUserId, x => x.AppUser, x => x.Office);
        if (customer == null)
        {
            throw new NotFoundException("Customer is not found");
        }

        var dto = _mapper.Map<CustomerUpdateDto>(customer);
        return new(dto);
    }

    public async Task<ResultDto> IncreaseBalance(Guid id, decimal amount)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new NotFoundException("Customer is not found");
        }

        customer.Balance += amount;
        await _repository.SaveChangesAsync();
        return new("Balance updated successfully");
    }

    public async Task<ResultDto> UpdateAsync(CustomerUpdateDto dto)
    {
        var customer = await _repository.GetByIdAsync(dto.Id);
        if (customer == null)
        {
            throw new NotFoundException("Customer is not found");
        }

        var user = await _userManager.FindByIdAsync(customer.AppUserId);
        if (user == null)
        {
            throw new NotFoundException("User is not found");
        }

        if (user.Email != dto.Email)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null && existing.Id != user.Id)
                throw new AlreadyExistException("Email already exists");

            user.Email = dto.Email;
        }

        if (user.UserName != dto.UserName)
        {
            var existingUsername = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUsername != null && existingUsername.Id != user.Id)
                throw new AlreadyExistException("Username already exists");

            user.UserName = dto.UserName;
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;

        var identityResult = await _userManager.UpdateAsync(user);
        if (!identityResult.Succeeded)
        {
            throw new UpdateFailException("User update failed");
        }

        customer.OfficeId = dto.OfficeId;
        _repository.Update(customer);
        await _repository.SaveChangesAsync();
        return new("Customer updated successfully");
    }
}
