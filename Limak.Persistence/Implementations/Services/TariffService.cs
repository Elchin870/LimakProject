using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.TariffDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class TariffService : ITariffService
{
    private readonly ITarifRepository _repository;
    private readonly IMapper _mapper;
    public TariffService(ITarifRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(TariffCreateDto dto)
    {
        var isOverlap = await _repository.AnyAsync(x =>
            x.CountryId == dto.CountryId &&
            x.DeliveryTypeId == dto.DeliveryTypeId &&
            x.ShipmentTypeId == dto.ShipmentTypeId &&
            x.MinWeight < dto.MaxWeight &&
            dto.MinWeight < x.MaxWeight);

        if (isOverlap)
            throw new AlreadyExistException("This tariff already exist");

        var tariff = _mapper.Map<Tariff>(dto);

        await _repository.AddAsync(tariff);
        await _repository.SaveChangesAsync();

        return new("Tariff created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var tariff = await _repository.GetByIdAsync(id);
        if (tariff == null)
            throw new NotFoundException("Tariff is not found");

        _repository.Delete(tariff);
        await _repository.SaveChangesAsync();
        return new("Tariff deleted successfully");
    }

    public async Task<ResultDto<List<TariffGetDto>>> GetAllAsync()
    {
        var tariffs = await _repository.GetAll()
        .Include(x => x.Country)
        .Include(x => x.DeliveryType)
        .Include(x => x.ShipmentType)
        .ToListAsync();

        var dtos = _mapper.Map<List<TariffGetDto>>(tariffs);
        return new(dtos);
    }

    public async Task<ResultDto<TariffGetDto>> GetByIdAsync(Guid id)
    {
        var tariff = await _repository.GetByIdAsync(id);
        if (tariff == null)
            throw new NotFoundException("Tariff is not found");

        var dto = _mapper.Map<TariffGetDto>(tariff);
        return new(dto);
    }

    public async Task<ResultDto<TariffUpdateDto>> GetUpdateDto(Guid id)
    {
        var tariff = await _repository.GetByIdAsync(id);
        if (tariff == null)
            throw new NotFoundException("Tariff is not found");

        var dto = _mapper.Map<TariffUpdateDto>(tariff);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(TariffUpdateDto dto)
    {
        var tariff = await _repository.GetByIdAsync(dto.Id);
        if (tariff == null)
            throw new NotFoundException("Tariff is not found");

        var isOverlap = await _repository.AnyAsync(x =>
            x.Id != dto.Id &&
            x.CountryId == dto.CountryId &&
            x.DeliveryTypeId == dto.DeliveryTypeId &&
            x.ShipmentTypeId == dto.ShipmentTypeId &&
            x.MinWeight < dto.MaxWeight &&
            dto.MinWeight < x.MaxWeight);

        if (isOverlap)
        {
            throw new AlreadyExistException("This tariff is already exist");
        }

        tariff = _mapper.Map(dto, tariff);

        _repository.Update(tariff);
        await _repository.SaveChangesAsync();

        return new("Tariff updated successfully");
    }
}
