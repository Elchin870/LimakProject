using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICloudinaryService _cloudinaryService;
    public AnnouncementService(IAnnouncementRepository repository, IMapper mapper, ICloudinaryService cloudinaryService)
    {
        _repository = repository;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResultDto> CreateAsync(AnnouncementCreateDto dto)
    {
        var announcement = _mapper.Map<Announcement>(dto);

        var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
        announcement.ImagePath = imagePath;

        await _repository.AddAsync(announcement);
        await _repository.SaveChangesAsync();
        return new("Announcement created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var announcement = await _repository.GetByIdAsync(id);
        if (announcement is null)
            throw new NotFoundException("Announcement is not found");

        _repository.Delete(announcement);
        await _repository.SaveChangesAsync();

        await _cloudinaryService.FileDeleteAsync(announcement.ImagePath);
        return new("Announcement deleted successfully");
    }

    public async Task<ResultDto<List<AnnouncementGetDto>>> GetAllAsync()
    {
        var announcements = await _repository.GetAll().ToListAsync();

        var dtos = _mapper.Map<List<AnnouncementGetDto>>(announcements);

        return new(dtos);
    }

    public async Task<ResultDto<AnnouncementGetDto>> GetByIdAsync(Guid id)
    {
        var announcement = await _repository.GetByIdAsync(id);
        if (announcement is null)
            throw new NotFoundException("Announcement is not found");

        var dto = _mapper.Map<AnnouncementGetDto>(announcement);
        return new(dto);
    }

    public async Task<ResultDto<AnnouncementUpdateDto>> GetUpdateDto(Guid id)
    {
        var announcement = await _repository.GetByIdAsync(id);
        if (announcement is null)
            throw new NotFoundException("Announcement is not found");

        var dto = _mapper.Map<AnnouncementUpdateDto>(announcement);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(AnnouncementUpdateDto dto)
    {
        var announcement = await _repository.GetByIdAsync(dto.Id);
        if (announcement is null)
            throw new NotFoundException("Announcement is not found");

        announcement = _mapper.Map(dto, announcement);

        if (dto.Image is not null)
        {
            await _cloudinaryService.FileDeleteAsync(announcement.ImagePath);
            var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
            announcement.ImagePath = imagePath;
        }

        _repository.Update(announcement);
        await _repository.SaveChangesAsync();

        return new("Announcement updated successfully");
    }
}
