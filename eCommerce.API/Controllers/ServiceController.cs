using eCommerce.Core.DTO.Service;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // ✅ GET: api/Service
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceService.GetAllAsync();

            if (services == null || !services.Any())
                return NotFound("No services found.");

            return Ok(services);
        }

        // ✅ GET: api/Service/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);

            if (service == null)
                return NotFound($"Service with ID {id} not found.");

            return Ok(service);
        }

        // ✅ POST: api/Service
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _serviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ServiceId }, created);
        }

        // ✅ PUT: api/Service/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _serviceService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Service with ID {id} not found.");

            return Ok(updated);
        }

        // ✅ DELETE: api/Service/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Service with ID {id} not found.");

            return NoContent();
        }

        // ✅ PATCH: api/Service/5/toggle
        [HttpPatch("{id:int}/toggle")]
        public async Task<IActionResult> ToggleActive(int id, [FromQuery] bool isActive)
        {
            var success = await _serviceService.ToggleActiveAsync(id, isActive);
            if (!success)
                return NotFound($"Service with ID {id} not found.");

            return Ok(new { ServiceId = id, IsActive = isActive });
        }
    }
}
