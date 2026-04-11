using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.ShopDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class ShopService : IShopService
{
    private readonly IShopRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICloudinaryService _cloudinaryService;
    public ShopService(IShopRepository repository, IMapper mapper, ICloudinaryService cloudinaryService)
    {
        _repository = repository;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResultDto> CreateAsync(ShopCreateDto dto)
    {
        var isExistShop = await _repository.AnyAsync(x => x.Name == dto.Name);
        if (isExistShop)
            throw new AlreadyExistException("This shop already exist");

        var shop = _mapper.Map<Shop>(dto);

        var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
        shop.ImagePath = imagePath;

        await _repository.AddAsync(shop);
        await _repository.SaveChangesAsync();

        return new("Shop created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var shop = await _repository.GetByIdAsync(id);

        if (shop is null)
            throw new NotFoundException("Shop is not found");


        _repository.Delete(shop);
        await _repository.SaveChangesAsync();

        await _cloudinaryService.FileDeleteAsync(shop.ImagePath);

        return new("Shop deleted successfully");
    }

    public async Task<ResultDto<List<ShopGetDto>>> GetAllAsync()
    {
        var shops = await _repository.GetAll().Include(s => s.Category).ToListAsync();

        var dtos = _mapper.Map<List<ShopGetDto>>(shops);

        return new(dtos);
    }

    public async Task<ResultDto<ShopGetDto>> GetByIdAsync(Guid id)
    {
        var shop = await _repository.GetByIdAsync(id);
        if (shop is null)
            throw new NotFoundException("Shop is not found");

        var dto = _mapper.Map<ShopGetDto>(shop);
        return new(dto);
    }

    public async Task<ResultDto<ShopUpdateDto>> GetUpdateDto(Guid id)
    {
        var shop = await _repository.GetByIdAsync(id);
        if (shop is null)
            throw new NotFoundException("Shop is not found");

        var dto = _mapper.Map<ShopUpdateDto>(shop);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(ShopUpdateDto dto)
    {
        var shop = await _repository.GetByIdAsync(dto.Id);

        if (shop is null)
            throw new NotFoundException("Shop is not found");

        var isExistShop = await _repository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id);
        if (isExistShop)
            throw new AlreadyExistException("This shop already exist");

        shop = _mapper.Map(dto, shop);

        if (dto.Image is not null)
        {
            await _cloudinaryService.FileDeleteAsync(shop.ImagePath);
            var imagePath = await _cloudinaryService.FileCreateAsync(dto.Image);
            shop.ImagePath = imagePath;
        }

        _repository.Update(shop);
        await _repository.SaveChangesAsync();

        return new("Shop updated successfully");
    }
}
