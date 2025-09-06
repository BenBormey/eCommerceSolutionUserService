using eCommerce.Core.DTO.CleanerAvailability;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CleanerAvailabilityController : ControllerBase
    {
        private readonly ICleanerAvailabilityService _service;

        public CleanerAvailabilityController(ICleanerAvailabilityService service)
        {
            _service = service;
        }

        // GET: api/CleanerAvailability/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CleanerAvailabilityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound($"Availability {id} not found.");
            return Ok(item);
        }

        // GET: api/CleanerAvailability/cleaner/{cleanerId}?from=2025-09-06&to=2025-09-10
        [HttpGet("cleaner/{cleanerId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<CleanerAvailabilityDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCleaner(
            Guid cleanerId,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        {
            var items = await _service.GetByCleanerAsync(cleanerId, from, to);
            return Ok(items); // empty array if none
        }

        // GET: api/CleanerAvailability/free?cleanerId=...&date=2025-09-06&startTime=09:00&endTime=11:00
        [HttpGet("free")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsFree(
            [FromQuery] Guid cleanerId,
            [FromQuery] DateOnly date,
            [FromQuery] TimeOnly startTime,
            [FromQuery] TimeOnly endTime)
        {
            var free = await _service.IsCleanerFreeAsync(cleanerId, date, startTime, endTime);
            return Ok(new { cleanerId, date, startTime, endTime, free });
        }

        // POST: api/CleanerAvailability
        [HttpPost]
        [ProducesResponseType(typeof(CleanerAvailabilityDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CleanerAvailabilityCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.EndTime <= dto.StartTime) return BadRequest("EndTime must be after StartTime.");

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.AvailabilityId }, created);
        }

        // PUT: api/CleanerAvailability/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CleanerAvailabilityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CleanerAvailabilityUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.AvailabilityId == 0) dto.AvailabilityId = id;
            else if (dto.AvailabilityId != id) return BadRequest("Route id and body AvailabilityId mismatch.");
            if (dto.EndTime <= dto.StartTime) return BadRequest("EndTime must be after StartTime.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated is null) return NotFound($"Availability {id} not found.");
            return Ok(updated);
        }

        // DELETE: api/CleanerAvailability/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound($"Availability {id} not found.");
            return NoContent();
        }
    }
}

