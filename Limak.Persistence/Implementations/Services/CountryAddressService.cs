using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CountryAddressDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class CountryAddressService : ICountryAddressService
{
    private readonly ICountryAddressRepository _repository;
    private readonly IMapper _mapper;
    public CountryAddressService(ICountryAddressRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(CountryAddressCreateDto dto)
    {
        var isExistCountryAddress = await _repository.AnyAsync(x => x.City == dto.City && x.State == dto.State && x.Address == dto.Address);
        if (isExistCountryAddress)
        {
            throw new AlreadyExistException("This Address is already exist");
        }

        var address = _mapper.Map<CountryAddress>(dto);
        await _repository.AddAsync(address);
        await _repository.SaveChangesAsync();
        return new("Address created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var address = await _repository.GetAsync(x => x.Id == id);
        if (address is null)
        {
            throw new NotFoundException("Address is not found");
        }

        _repository.Delete(address);
        await _repository.SaveChangesAsync();
        return new("Address deleted successfully");
    }

    public async Task<ResultDto<List<CountryAddressGetDto>>> GetAllAsync()
    {
        var addresses = await _repository.GetAll().Include(x => x.Country).ToListAsync();
        var dtos = _mapper.Map<List<CountryAddressGetDto>>(addresses);
        return new(dtos);
    }

    public async Task<ResultDto<CountryAddressGetDto>> GetByIdAsync(Guid id)
    {
        var address = await _repository.GetByIdAsync(id);
        if (address is null)
        {
            throw new NotFoundException("Address is not found");
        }

        var dto = _mapper.Map<CountryAddressGetDto>(address);
        return new(dto);
    }

    public async Task<ResultDto<CountryAddressUpdateDto>> GetUpdateDto(Guid id)
    {
        var address = await _repository.GetByIdAsync(id);
        if (address is null)
        {
            throw new NotFoundException("Address is not found");
        }

        var dto = _mapper.Map<CountryAddressUpdateDto>(address);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(CountryAddressUpdateDto dto)
    {
        var address = await _repository.GetByIdAsync(dto.Id);
        if (address is null)
        {
            throw new NotFoundException("Address is not found");
        }

        var isExistCountryAddress = await _repository.AnyAsync(x => x.City == dto.City && x.State == dto.State && x.Address == dto.Address && x.Id != dto.Id);
        if (isExistCountryAddress)
        {
            throw new AlreadyExistException("This Address is already exist");
        }

        address = _mapper.Map(dto, address);
        _repository.Update(address);
        await _repository.SaveChangesAsync();
        return new("Address updated successfully");
    }
}
