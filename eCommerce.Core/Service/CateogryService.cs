using eCommerce.Core.DTO.Category;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryCreateDTO dto)
        {
            return await _repository.CreateCategoryAsync(dto);
        }

        public async Task<IReadOnlyList<CategoryDTO>> GetAllCategoriesAsync()
        {
            return await _repository.GetAllCategoriesAsync();
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _repository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<CategoryDTO?> UpdateCategoryAsync(CategoryUpdateDTO dto)
        {
            return await _repository.UpdateCategoryAsync(dto);
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            return await _repository.DeleteCategoryAsync(categoryId);
        }
    }
}
