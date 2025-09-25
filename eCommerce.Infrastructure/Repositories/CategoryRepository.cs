using Dapper;
using eCommerce.Core.DTO.Category;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperDbContext _db;
        public CategoryRepository(DapperDbContext db) => _db = db;

        // CREATE -------------------------------------------------------------
        public async Task<Guid> CreateAsync(CategoryCreateDTO dto)
        {
            const string sql = @"
INSERT INTO categories (category_id, name, description, created_at)
VALUES (@CategoryId, @CategoryName, @CategoryDescription, @CreatedAt)
RETURNING category_id;";

            var p = new
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription,
                CreatedAt = DateTime.UtcNow
            };

            // Option A:
            
            return await _db.DbConnection.ExecuteScalarAsync<Guid>(sql, p);

            // Option B (if you used DataSource):
            // await using var conn = await _db.OpenConnectionAsync();
            // return await conn.ExecuteScalarAsync<Guid>(sql, p);
        }

        // UPDATE -------------------------------------------------------------
        public async Task<bool> UpdateAsync(CategoryUpdateDTO dto)
        {
            const string sql = @"
UPDATE categories
SET name = @CategoryName,
    description = @CategoryDescription,
    updated_at = @UpdatedAt
WHERE category_id = @CategoryId;";

            var p = new
            {
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription,
                UpdatedAt = DateTime.UtcNow
            };

         
            var rows = await _db.DbConnection.ExecuteAsync(sql, p);
            return rows > 0;
        }

        // DELETE -------------------------------------------------------------
        public async Task<bool> DeleteAsync(Guid categoryId)
        {
            const string sql = @"DELETE FROM categories WHERE category_id = @categoryId;";
            
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { categoryId });
            return rows > 0;
        }

        // READ ONE -----------------------------------------------------------
        public async Task<Category?> GetByIdAsync(Guid categoryId)
        {
            const string sql = @"
SELECT  category_id   AS ""CategoryId"",
        name          AS ""CategoryName"",
        description   AS ""CategoryDescription"",
        created_at    AS ""CreateAt"",
        updated_at    AS ""UpdateAt""
FROM categories
WHERE category_id = @categoryId;";

           
            return await _db.DbConnection.QueryFirstOrDefaultAsync<Category>(sql, new { categoryId });
        }

        // READ ALL -----------------------------------------------------------
        public async Task<IReadOnlyList<Category>> GetAllAsync()
        {
            const string sql = @"
SELECT  category_id   AS ""CategoryId"",
        name          AS ""CategoryName"",
        description   AS ""CategoryDescription"",
        created_at    AS ""CreateAt"",
        updated_at    AS ""UpdateAt""
FROM categories
ORDER BY name;";

           
            var rows = await _db.DbConnection.QueryAsync<Category>(sql);
            return rows.AsList(); // materialize before disposing
        }

        // EXISTS -------------------------------------------------------------
        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            const string sql = @"
SELECT EXISTS (
  SELECT 1 FROM categories
  WHERE LOWER(name) = LOWER(@name)
    AND (@excludeId IS NULL OR category_id <> @excludeId)
);";

            
            return await _db.DbConnection.ExecuteScalarAsync<bool>(sql, new { name, excludeId });
        }

        // COUNT --------------------------------------------------------------
        public async Task<int> CountAsync()
        {
            const string sql = @"SELECT COUNT(*) FROM categories;";
         
            return await _db.DbConnection.ExecuteScalarAsync<int>(sql);
        }
    }
}
