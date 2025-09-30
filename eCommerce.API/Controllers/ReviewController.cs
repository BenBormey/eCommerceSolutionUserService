using eCommerce.Core.DTO.Report;
using eCommerce.Core.DTO.Review;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        // GET: api/Review/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound($"Review {id} not found.");
            return Ok(item);
        }

       
        [HttpGet("booking/{bookingId:guid}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByBooking(Guid bookingId)
        {
            var item = await _service.GetByBookingAsync(bookingId);
            if (item is null) return NotFound($"No review for booking {bookingId}.");
            return Ok(item);
        }

       
        [HttpGet("cleaner/{cleanerId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCleaner(Guid cleanerId, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            var items = await _service.GetByCleanerAsync(cleanerId, limit, offset);
            return Ok(items); // empty array if none
        }

      
        [HttpGet("customer/{customerId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomer(Guid customerId, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            var items = await _service.GetByCustomerAsync(customerId, limit, offset);
            return Ok(items);
        }


        [HttpGet("cleaner/{cleanerId:guid}/summary")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCleanerSummary(Guid cleanerId)
        {
            var (avg, count) = await _service.GetCleanerRatingSummaryAsync(cleanerId);
            return Ok(new { cleanerId, average = avg, count });
        }

        // POST: api/Review
        [HttpPost]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ReviewId }, created);
        }

        // PUT: api/Review/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ReviewUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.ReviewId == 0) dto.ReviewId = id;
            else if (dto.ReviewId != id) return BadRequest("Route id and body ReviewId mismatch.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated is null) return NotFound($"Review {id} not found.");
            return Ok(updated);
        }

        // DELETE: api/Review/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound($"Review {id} not found.");
            return NoContent();
        }
      

    }
}

