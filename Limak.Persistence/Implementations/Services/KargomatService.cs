using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.KargomatDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class KargomatService : IKargomatService
{
    private readonly IKargomatRepository _repository;
    private readonly IMapper _mapper;
    public KargomatService(IKargomatRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(KargomatCreateDto dto)
    {
        var isExist = await _repository.AnyAsync(x => x.ShortAddress == dto.ShortAddress && x.FullAddress == dto.FullAddress);
        if (isExist)
        {
            throw new AlreadyExistException("This kargomat already exist");
        }

        var kargomat = _mapper.Map<Kargomat>(dto);
        await _repository.AddAsync(kargomat);
        await _repository.SaveChangesAsync();
        return new("Kargomat created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var kargomat = await _repository.GetByIdAsync(id);
        if (kargomat is null)
        {
            throw new NotFoundException("Kargomat not found");
        }

        _repository.Delete(kargomat);
        await _repository.SaveChangesAsync();
        return new("Kargomat deleted successfully");
    }

    public async Task<ResultDto<List<KargomatGetDto>>> GetAllAsync()
    {
        var kargomats = await _repository.GetAll().ToListAsync();
        var dtos = _mapper.Map<List<KargomatGetDto>>(kargomats);
        return new(dtos);
    }

    public async Task<ResultDto<KargomatGetDto>> GetByIdAsync(Guid id)
    {
        var kargomat = await _repository.GetByIdAsync(id);
        if (kargomat is null)
        {
            throw new NotFoundException("Kargomat not found");
        }

        var dto = _mapper.Map<KargomatGetDto>(kargomat);
        return new(dto);

    }

    public async Task<ResultDto<KargomatUpdateDto>> GetUpdateDto(Guid id)
    {
        var kargomat = await _repository.GetByIdAsync(id);
        if (kargomat is null)
        {
            throw new NotFoundException("Kargomat not found");
        }

        var dto = _mapper.Map<KargomatUpdateDto>(kargomat);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(KargomatUpdateDto dto)
    {
        var kargomat = _repository.GetByIdAsync(dto.Id).Result;
        if (kargomat is null)
        {
            throw new NotFoundException("Kargomat not found");
        }
        var isExist = await _repository.AnyAsync(x => x.Id != dto.Id && x.ShortAddress == dto.ShortAddress && x.FullAddress == dto.FullAddress);
        if (isExist)
        {
            throw new AlreadyExistException("This kargomat already exist");
        }

        kargomat = _mapper.Map(dto, kargomat);
        _repository.Update(kargomat);
        await _repository.SaveChangesAsync();
        return new("Kargomat updated successfully");
    }
}
