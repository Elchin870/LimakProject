using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShipmentTypeDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class ShipmentTypeService : IShipmentTypeService
{
    private readonly IShipmentTypeRepository _repository;
    private readonly IMapper _mapper;

    public ShipmentTypeService(IShipmentTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(ShipmentTypeCreateDto dto)
    {
        var isExist = await _repository.AnyAsync(x => x.Name == dto.Name);
        if (isExist)
            throw new AlreadyExistException("This shipment type already exist");

        var entity = _mapper.Map<ShipmentType>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return new("Shipment type created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException("Shipment type not found");

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();

        return new("Shipment type deleted successfully");
    }

    public async Task<ResultDto<List<ShipmentTypeGetDto>>> GetAllAsync()
    {
        var entities = await _repository.GetAll().ToListAsync();
        var dtos = _mapper.Map<List<ShipmentTypeGetDto>>(entities);

        return new(dtos);
    }

    public async Task<ResultDto<ShipmentTypeGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException("Shipment type not found");

        var dto = _mapper.Map<ShipmentTypeGetDto>(entity);
        return new(dto);
    }

    public async Task<ResultDto<ShipmentTypeUpdateDto>> GetUpdateDto(Guid id)
    {
        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException("Shipment type not found");

        var dto = _mapper.Map<ShipmentTypeUpdateDto>(entity);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(ShipmentTypeUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null)
            throw new NotFoundException("Shipment type not found");

        var isExist = await _repository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id);
        if (isExist)
            throw new AlreadyExistException("This shipment type already exist");

        entity = _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return new("Shipment type updated successfully");
    }
}
