using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CountryDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _repository;
    private readonly IMapper _mapper;
    public CountryService(ICountryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(CountryCreateDto dto)
    {
        var isExistCountry = await _repository.AnyAsync(x => x.Name == dto.Name);
        if (isExistCountry)
        {
            throw new AlreadyExistException("This country already exist");
        }

        var country = _mapper.Map<Country>(dto);
        await _repository.AddAsync(country);
        await _repository.SaveChangesAsync();
        return new("Country created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var country = await _repository.GetAsync(x => x.Id == id);
        if (country is null)
        {
            throw new NotFoundException("Country not found");
        }
        _repository.Delete(country);
        await _repository.SaveChangesAsync();
        return new("Country deleted successfully");
    }

    public async Task<ResultDto<List<CountryGetDto>>> GetAllAsync()
    {
        var countries = await _repository.GetAll().ToListAsync();
        var countryDtos = _mapper.Map<List<CountryGetDto>>(countries);
        return new(countryDtos);
    }

    public async Task<ResultDto<CountryGetDto>> GetByIdAsync(Guid id)
    {
        var country = await _repository.GetAsync(x => x.Id == id);
        if (country is null)
        {
            throw new NotFoundException("Country not found");
        }
        var countryDto = _mapper.Map<CountryGetDto>(country);
        return new(countryDto);
    }

    public async Task<ResultDto<CountryUpdateDto>> GetUpdateDto(Guid id)
    {
        var country = await _repository.GetAsync(x => x.Id == id);
        if (country is null)
        {
            throw new NotFoundException("Country not found");
        }
        var dto = _mapper.Map<CountryUpdateDto>(country);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(CountryUpdateDto dto)
    {
        var country = await _repository.GetByIdAsync(dto.Id);
        if (country is null)
        {
            throw new NotFoundException("Country not found");
        }

        var isExistCountry = await _repository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id);
        if (isExistCountry)
        {
            throw new AlreadyExistException("This country already exist");
        }

        country = _mapper.Map(dto, country);
        _repository.Update(country);
        await _repository.SaveChangesAsync();
        return new("Country updated successfully");
    }
}
