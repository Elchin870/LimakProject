using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.DeliveryTypeDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class DeliveryTypeService : IDeliveryTypeService
{
    private readonly IDeliveryTypeRepository _repository;
    private readonly IMapper _mapper;

    public DeliveryTypeService(IDeliveryTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(DeliveryTypeCreateDto dto)
    {
        var isExist = await _repository.AnyAsync(x => x.Name == dto.Name);
        if (isExist)
            throw new AlreadyExistException("This delivery type already exist");

        var entity = _mapper.Map<DeliveryType>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return new("Delivery type created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException("Delivery type not found");

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();

        return new("Delivery type deleted successfully");
    }

    public async Task<ResultDto<List<DeliveryTypeGetDto>>> GetAllAsync()
    {
        var entities = await _repository.GetAll().ToListAsync();
        var dtos = _mapper.Map<List<DeliveryTypeGetDto>>(entities);

        return new(dtos);
    }

    public async Task<ResultDto<DeliveryTypeGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException("Delivery type not found");

        var dto = _mapper.Map<DeliveryTypeGetDto>(entity);
        return new(dto);
    }

    public async Task<ResultDto<DeliveryTypeUpdateDto>> GetUpdateDto(Guid id)
    {
        var entity = await _repository.GetAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException("Delivery type not found");

        var dto = _mapper.Map<DeliveryTypeUpdateDto>(entity);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(DeliveryTypeUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null)
            throw new NotFoundException("Delivery type not found");

        var isExist = await _repository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id);
        if (isExist)
            throw new AlreadyExistException("This delivery type already exist");

        entity = _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return new("Delivery type updated successfully");
    }
}
