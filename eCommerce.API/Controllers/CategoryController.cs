using eCommerce.Core.DTO.Category;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryCreateDTO dto)
        {
            var created = await _service.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = created.CategoryId }, created);
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/Category/{id}
        [HttpGet("{categoryId:guid}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(Guid categoryId)
        {
            var category = await _service.GetCategoryByIdAsync(categoryId);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // PUT: api/Category/{id}
        [HttpPut("{categoryId:guid}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(Guid categoryId, [FromBody] CategoryUpdateDTO dto)
        {
            if (categoryId != dto.CategoryId)
                return BadRequest("Category ID mismatch");

            var updated = await _service.UpdateCategoryAsync(dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategory(Guid categoryId)
        {
            var deleted = await _service.DeleteCategoryAsync(categoryId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
       
    }
}
