using Limak.Application.Dtos.CategoryDtos;
using Limak.Application.Dtos.ResultDtos;

namespace Limak.Application.Abstractions.Services;

public interface ICategoryService
{
    Task<ResultDto> CreateAsync(CategoryCreateDto dto);
    Task<ResultDto> UpdateAsync(CategoryUpdateDto dto);
    Task<ResultDto<CategoryUpdateDto>> GetUpdateDto(Guid id);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<CategoryGetDto>>> GetAllAsync();
    Task<ResultDto<CategoryGetDto>> GetByIdAsync(Guid id);
}
