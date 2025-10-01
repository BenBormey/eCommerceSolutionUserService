using eCommerce.Core.DTO.Booking;
using eCommerce.Core.DTO.Report;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Common;
using System.Security.Claims;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }


        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound($"Booking {id} not found.");
            return Ok(item);
        }

        [HttpGet("my-booking")]
        public async Task<IActionResult> GetByBooking(Guid userid)
        {
           

      
      

            // Query DB ដើម្បីយក booking របស់ userId
            var bookings = await _service.GetByIdAsync(userid);

            return Ok(bookings);
        }

        [HttpGet("Booking-Customer")]
        [ProducesResponseType(typeof(IEnumerable<BookingDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookingCustomer(
      [FromQuery(Name = "customid")] Guid customerId)   // bind the exact query key
        {
            var result = await _service.GetMyBooking(customerId); 
            return Ok(result ?? new List<BookingDTO>());         
        }





        [HttpGet("customer/{customerId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<BookingDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomer(
            Guid customerId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var items = await _service.GetByCustomerAsync(customerId, from, to);
            return Ok(items);
        }

        [HttpGet("cleaner/{cleanerId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<BookingDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCleaner(
            Guid cleanerId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var items = await _service.GetByCleanerAsync(cleanerId, from, to);
            return Ok(items);
        }
       



 
        [HttpPost]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] BookingCreateDTO dto)
      {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.BookingId }, created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookingUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.BookingId == Guid.Empty) dto.BookingId = id;
            else if (dto.BookingId != id) return BadRequest("Route id and body BookingId mismatch.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated is null) return NotFound($"Booking {id} not found.");
            return Ok(updated);
        }


        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound($"Booking {id} not found.");
            return NoContent();
        }



        [HttpPatch("{id:guid}/confirm")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Confirm(Guid id)
        {
            // get cleaner id from JWT
            var cleanerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst(ClaimTypes.Name)?.Value;
            if (!Guid.TryParse(cleanerIdClaim, out var cleanerId))
                return Unauthorized("Invalid cleaner identity.");

            var ok = await _service.ConfirmAsync(id, cleanerId);   // pass cleanerId
            if (!ok) return Conflict("Booking is not Pending or already confirmed.");

           
            return Ok();
        }





        [HttpPatch("{id:guid}/complete")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Complete(Guid id)
        {
            var cleanerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst(ClaimTypes.Name)?.Value;
            if (!Guid.TryParse(cleanerIdClaim, out var cleanerId))
                return Unauthorized("Invalid cleaner identity.");
            var ok = await _service.CompleteAsync(id,cleanerId);
            if (!ok) return NotFound($"Booking {id} not found.");
            return Ok();
        }

   
        [HttpPatch("{id:guid}/cancel")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cleanerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst(ClaimTypes.Name)?.Value;
            if (!Guid.TryParse(cleanerIdClaim, out var cleanerId))
                return Unauthorized("Invalid cleaner identity.");
            var ok = await _service.CancelAsync(id, cleanerId);
            if (!ok) return NotFound($"Booking {id} not found.");
            var b = await _service.GetByIdAsync(id);
            return Ok(b);
        }


        //[Authorize(Roles = "Cleaner")]
        [HttpGet("for-cleaner")]
        [ProducesResponseType(typeof(IEnumerable<BookingDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListForCleaner( string status = "pending",
                                                 DateTime? from = null,
                                                 DateTime? to = null)
        {

            var cleanerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? User.FindFirst(ClaimTypes.Name)?.Value;
            if (!Guid.TryParse(cleanerIdClaim, out var cleanerId))
                return Unauthorized("Invalid cleaner identity.");


            var items = await _service.ListForCleanerAsync(status, from, to, cleanerId);
            return Ok(items);
        }
  

    }
}

