using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.PartnerDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class PartnerService : IPartnerService
{
    private readonly IPartnerRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICloudinaryService _cloudinaryService;
    public PartnerService(IPartnerRepository repository, IMapper mapper, ICloudinaryService cloudinaryService)
    {
        _repository = repository;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResultDto> CreateAsync(PartnerCreateDto dto)
    {
        var isExistPartner = await _repository.AnyAsync(x => x.Name == dto.Name);
        if (isExistPartner)
        {
            throw new AlreadyExistException("This partner already exist");
        }

        var partner = _mapper.Map<Partner>(dto);

        var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
        partner.ImagePath = imagePath;

        await _repository.AddAsync(partner);
        await _repository.SaveChangesAsync();

        return new("Partner created successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var partner = await _repository.GetByIdAsync(id);
        if (partner is null)
        {
            throw new NotFoundException("Partner not found");
        }

        _repository.Delete(partner);
        await _repository.SaveChangesAsync();

        await _cloudinaryService.FileDeleteAsync(partner.ImagePath);

        return new("Partner deleted successfully.");
    }

    public async Task<ResultDto<List<PartnerGetDto>>> GetAllAsync()
    {
        var partners = await _repository.GetAll().ToListAsync();
        var dtos = _mapper.Map<List<PartnerGetDto>>(partners);
        return new(dtos);
    }

    public async Task<ResultDto<PartnerGetDto>> GetByIdAsync(Guid id)
    {
        var partner = await _repository.GetByIdAsync(id);
        if (partner is null)
        {
            throw new NotFoundException("Partner not found");
        }

        var dto = _mapper.Map<PartnerGetDto>(partner);
        return new(dto);
    }

    public async Task<ResultDto<PartnerUpdateDto>> GetUpdateDto(Guid id)
    {
        var partner = await _repository.GetByIdAsync(id);
        if (partner is null)
        {
            throw new NotFoundException("Partner not found");
        }

        var dto = _mapper.Map<PartnerUpdateDto>(partner);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(PartnerUpdateDto dto)
    {
        var partner = await _repository.GetByIdAsync(dto.Id);
        if (partner is null)
        {
            throw new NotFoundException("Partner not found");
        }

        var isExistPartner = await _repository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id);
        if (isExistPartner)
        {
            throw new AlreadyExistException("This partner already exist");
        }

        partner = _mapper.Map(dto, partner);
        if (dto.Image is not null)
        {
            await _cloudinaryService.FileDeleteAsync(partner.ImagePath);
            var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
            partner.ImagePath = imagePath;
        }

        _repository.Update(partner);
        await _repository.SaveChangesAsync();
        return new("Partner updated successfully.");
    }
}
