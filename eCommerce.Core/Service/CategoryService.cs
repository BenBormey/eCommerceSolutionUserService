using AutoMapper;
using eCommerce.Core.DTO.Category;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> CreateAsync(CategoryCreateDTO dto)
        {
            // ✅ Validate unique name
            if (await _repo.ExistsByNameAsync(dto.CategoryName))
                throw new InvalidOperationException("Category name already exists.");

            // Map DTO → Entity
            var entity = _mapper.Map<CategoryCreateDTO>(dto);

            // Insert into DB
            var id = await _repo.CreateAsync(entity);

            // Get back full entity
            var created = await _repo.GetByIdAsync(id)
                          ?? throw new Exception("Failed to fetch created category.");

            return _mapper.Map<CategoryDTO>(created);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<CategoryDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<CategoryDTO>(entity);
        }

        public async Task<CategoryDTO?> UpdateAsync(CategoryUpdateDTO dto)
        {
            // ✅ Validate unique name (exclude current ID)
            if (await _repo.ExistsByNameAsync(dto.CategoryName, dto.CategoryId))
                throw new InvalidOperationException("Category name already exists.");

            var entity = _mapper.Map<CategoryUpdateDTO>(dto);

            var success = await _repo.UpdateAsync(entity);
            if (!success) return null;

            var updated = await _repo.GetByIdAsync(dto.CategoryId);
            return updated is null ? null : _mapper.Map<CategoryDTO>(updated);
        }

        public async Task<IReadOnlyList<CategoryDTO>> GetAllAsync()
        {
            var rows = await _repo.GetAllAsync();
            return _mapper.Map<IReadOnlyList<CategoryDTO>>(rows);
        }
    }
}
