using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.CategoryDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limak.Persistence.Implementations.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(CategoryCreateDto dto)
    {
        var isExistCategory = await _repository.AnyAsync(x => x.Name == dto.Name);
        if (isExistCategory)
        {
            throw new AlreadyExistException("This category already exist");
        }

        var category = _mapper.Map<Category>(dto);
        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();
        return new("Category created successfully");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var category = await _repository.GetAsync(x => x.Id == id);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }
        _repository.Delete(category);
        await _repository.SaveChangesAsync();
        return new("Category deleted successfully");
    }

    public async Task<ResultDto<List<CategoryGetDto>>> GetAllAsync()
    {
        var categories = await _repository.GetAll().ToListAsync();
        var categoryDtos = _mapper.Map<List<CategoryGetDto>>(categories);
        return new(categoryDtos);
    }

    public async Task<ResultDto<CategoryGetDto>> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetAsync(x => x.Id == id);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }
        var categoryDto = _mapper.Map<CategoryGetDto>(category);
        return new(categoryDto);
    }

    public async Task<ResultDto<CategoryUpdateDto>> GetUpdateDto(Guid id)
    {
        var category = await _repository.GetAsync(x => x.Id == id);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }
        var dto = _mapper.Map<CategoryUpdateDto>(category);
        return new(dto);
    }

    public async Task<ResultDto> UpdateAsync(CategoryUpdateDto dto)
    {
        var category = await _repository.GetByIdAsync(dto.Id);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }

        var isExistCategory = await _repository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id);
        if (isExistCategory)
        {
            throw new AlreadyExistException("This category already exist");
        }

        category = _mapper.Map(dto, category);
        _repository.Update(category);
        await _repository.SaveChangesAsync();
        return new("Category updated successfully");
    }
}
