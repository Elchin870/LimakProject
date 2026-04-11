using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.OfficeDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class OfficeService : IOfficeService
{
    private readonly IOfficeRepository _repository;
    private readonly IMapper _mapper;

    public OfficeService(IOfficeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(OfficeCreateDto dto)
    {
        var isExistOffice = await _repository.AnyAsync(x => x.City == dto.City && x.MetroStation == dto.MetroStation);
        if (isExistOffice)
        {
            throw new AlreadyExistException("This office already exist");
        }

        var office = _mapper.Map<Office>(dto);
        await _repository.AddAsync(office);
        await _repository.SaveChangesAsync();

        return new("Office created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var office = await _repository.GetAsync(x => x.Id == id);
        if (office is null)
        {
            throw new NotFoundException("Office not found");
        }

        _repository.Delete(office);
        await _repository.SaveChangesAsync();
        return new("Office deleted successfully");
    }

    public async Task<ResultDto<List<OfficeGetDto>>> GetAllAsync()
    {
        var offices = await _repository.GetAll().ToListAsync();
        var officeDtos = _mapper.Map<List<OfficeGetDto>>(offices);
        return new(officeDtos);
    }

    public async Task<ResultDto<OfficeGetDto>> GetByIdAsync(Guid id)
    {
        var office = await _repository.GetAsync(x => x.Id == id);
        if (office is null)
        {
            throw new NotFoundException("Office not found");
        }

        var dto = _mapper.Map<OfficeGetDto>(office);
        return new(dto);
    }

    public async Task<ResultDto<OfficeUpdateDto>> GetUpdateDto(Guid id)
    {
        var office = await _repository.GetByIdAsync(id);
        if (office is null)
        {
            throw new NotFoundException("Office not found");
        }
        var dto = _mapper.Map<OfficeUpdateDto>(office);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(OfficeUpdateDto dto)
    {
        var office = await _repository.GetByIdAsync(dto.Id);
        if (office is null)
        {
            throw new NotFoundException("Office not found");
        }

        var isExistOffice = await _repository.AnyAsync(x => x.Id != dto.Id && x.City == dto.City && x.MetroStation == dto.MetroStation);
        if (isExistOffice)
        {
            throw new AlreadyExistException("This office already");
        }

        office = _mapper.Map(dto, office);
        _repository.Update(office);
        await _repository.SaveChangesAsync();

        return new("Office updated successfully");
    }
}
