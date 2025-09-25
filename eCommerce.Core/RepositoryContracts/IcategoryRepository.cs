// eCommerce.Core/RepositoryContracts/ICategoryRepository.cs
using eCommerce.Core.DTO.Category;
using eCommerce.Core.Entities;

namespace eCommerce.Core.RepositoryContracts
{
    public interface ICategoryRepository
    {
        Task<Guid> CreateAsync(CategoryCreateDTO entity);
        Task<bool> UpdateAsync(CategoryUpdateDTO entity);
        Task<bool> DeleteAsync(Guid categoryId);

        Task<Category?> GetByIdAsync(Guid categoryId);
        Task<IReadOnlyList<Category>> GetAllAsync();

        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
        Task<int> CountAsync();
    }
}
