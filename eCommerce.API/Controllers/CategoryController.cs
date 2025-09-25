using eCommerce.Core.DTO.Category;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        // ---------------- GET ALL ----------------
        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        // ---------------- GET BY ID ----------------
        /// <summary>
        /// Get category by Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        // ---------------- CREATE ----------------
        /// <summary>
        /// Create new category
        /// </summary>
 //       [Authorize(Roles = "Admin")] // only admin can create
        [HttpPost]
        //[ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // ---------------- UPDATE ----------------
        /// <summary>
        /// Update category
        /// </summary>
  //      [Authorize(Roles = "Admin")] // only admin can update
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDTO dto)
        {
            if (id != dto.CategoryId)
                return BadRequest("Id in route does not match body");

            try
            {
                var updated = await _service.UpdateAsync(dto);
                if (updated is null) return NotFound();

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // ---------------- DELETE ----------------
        /// <summary>
        /// Delete category
        /// </summary>
  //      [Authorize(Roles = "Admin")] // only admin can delete
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
