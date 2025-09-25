// eCommerce.Core/ServiceContracts/ICategoryService.cs
using eCommerce.Core.DTO.Category;

namespace eCommerce.Core.ServiceContracts
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateAsync(CategoryCreateDTO dto);
        Task<CategoryDTO?> UpdateAsync(CategoryUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<CategoryDTO?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<CategoryDTO>> GetAllAsync();
    }
}
