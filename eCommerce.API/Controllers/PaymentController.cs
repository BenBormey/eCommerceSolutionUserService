using eCommerce.Core.DTO.NewFolder;
using eCommerce.Core.DTO.Payment;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        // GET: api/Payment/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PaymentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound($"Payment {id} not found.");
            return Ok(item);
        }

        // GET: api/Payment/booking/{bookingId}
        [HttpGet("booking/{bookingId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<PaymentDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBooking(Guid bookingId)
        {
            var items = await _service.GetByBookingAsync(bookingId);
            return Ok(items); // empty array if none
        }

        // POST: api/Payment
        [HttpPost]
        [ProducesResponseType(typeof(PaymentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PaymentCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.PaymentId }, created);
        }

        // PUT: api/Payment/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PaymentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] PaymentUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.PaymentId == Guid.Empty) dto.PaymentId = id;
            else if (dto.PaymentId != id) return BadRequest("Route id and body PaymentId mismatch.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated is null) return NotFound($"Payment {id} not found.");
            return Ok(updated);
        }

        // PATCH: api/Payment/{id}/paid?paidAt=2025-09-07T09:00:00Z&transactionId=TX123
        [HttpPatch("{id:guid}/paid")]
        [ProducesResponseType(typeof(PaymentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkPaid(Guid id, [FromQuery] DateTime? paidAt, [FromQuery] string? transactionId)
        {
            var ok = await _service.MarkPaidAsync(id, paidAt, transactionId);
            if (!ok) return NotFound($"Payment {id} not found.");

            // return the fresh row
            var item = await _service.GetByIdAsync(id);
            return Ok(item);
        }

        // DELETE: api/Payment/{id}
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound($"Payment {id} not found.");
            return NoContent();
        }
    }
}
