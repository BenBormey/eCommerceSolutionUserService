using eCommerce.Core.DTO.Location;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        // GET: api/Location
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LocationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _locationService.GetAllAsync();
            if (items == null || !items.Any())
                return NotFound("No locations found.");
            return Ok(items);
        }

        // GET: api/Location/5
        [HttpGet("{id:int}", Name = nameof(GetById))]
        [ProducesResponseType(typeof(LocationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _locationService.GetByIdAsync(id);
            if (item == null)
                return NotFound($"Location with ID {id} not found.");
            return Ok(item);
        }

        // GET: api/Location/search?city=...&district=...&postalCode=...
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<LocationDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string? city, [FromQuery] string? district, [FromQuery] string? postalCode)
        {
            var items = await _locationService.SearchAsync(city, district, postalCode);
            return Ok(items);
        }

        // POST: api/Location
        [HttpPost]
        [ProducesResponseType(typeof(LocationDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] LocationCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _locationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.LocationId }, created);
        }

        // PUT: api/Location/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(LocationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] LocationUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // keep route/body ids consistent (service also guards)
            if (dto.LocationId == 0) dto.LocationId = id;
            if (dto.LocationId != id) return BadRequest("Route id and body LocationId mismatch.");

            var updated = await _locationService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Location with ID {id} not found.");

            return Ok(updated);
        }

        // DELETE: api/Location/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _locationService.DeleteAsync(id);
            if (!ok)
                return NotFound($"Location with ID {id} not found.");

            return NoContent();
        }
    }
}
