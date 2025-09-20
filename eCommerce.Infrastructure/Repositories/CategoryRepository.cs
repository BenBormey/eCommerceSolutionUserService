using Dapper;
using eCommerce.Core.DTO.Category;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperDbContext _db;

        public CategoryRepository(DapperDbContext db)
        {
            _db = db;
        }

        // Create
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryCreateDTO dto)
        {
            const string sql = @"
                INSERT INTO categories (category_id, name, description, created_at, updated_at)
                VALUES (@CategoryId, @Name, @Description, @CreatedAt, @UpdatedAt)
                RETURNING category_id AS CategoryId,
                          name,
                          description,
                          created_at AS CreatedAt,
                          updated_at AS UpdatedAt;";

            var parameters = new
            {
                CategoryId = Guid.NewGuid(),
                dto.Name,
                dto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _db.DbConnection.QueryFirstOrDefaultAsync<CategoryDTO>(sql, parameters);
        }

        // Get all
        public async Task<IReadOnlyList<CategoryDTO>> GetAllCategoriesAsync()
        {
            const string sql = @"
                SELECT category_id AS CategoryId,
                       name,
                       description,
                       created_at AS CreatedAt,
                       updated_at AS UpdatedAt
                FROM categories
                ORDER BY created_at DESC;";

            var result = await _db.DbConnection.QueryAsync<CategoryDTO>(sql);
            return result.ToList();
        }

        // Get by Id
        public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid categoryId)
        {
            const string sql = @"
                SELECT category_id AS CategoryId,
                       name,
                       description,
                       created_at AS CreatedAt,
                       updated_at AS UpdatedAt
                FROM categories
                WHERE category_id = @CategoryId;";

            return await _db.DbConnection.QueryFirstOrDefaultAsync<CategoryDTO>(sql, new { CategoryId = categoryId });
        }

        // Update
        public async Task<CategoryDTO?> UpdateCategoryAsync(CategoryUpdateDTO dto)
        {
            const string sql = @"
                UPDATE categories
                SET name = @Name,
                    description = @Description,
                    updated_at = @UpdatedAt
                WHERE category_id = @CategoryId
                RETURNING category_id AS CategoryId,
                          name,
                          description,
                          created_at AS CreatedAt,
                          updated_at AS UpdatedAt;";

            var parameters = new
            {
                dto.CategoryId,
                dto.Name,
                dto.Description,
                UpdatedAt = DateTime.UtcNow
            };

            return await _db.DbConnection.QueryFirstOrDefaultAsync<CategoryDTO>(sql, parameters);
        }

        // Delete
        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            const string sql = @"DELETE FROM categories WHERE category_id = @CategoryId;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { CategoryId = categoryId });
            return rows > 0;
        }
    }
}
