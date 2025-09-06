using eCommerce.Core.DTO.Notification;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
          private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notification/user/{userId}?limit=&offset=
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<NotificationDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUser(Guid userId, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            var items = await _notificationService.GetByUserAsync(userId, limit, offset);
            return Ok(items); // return empty array if none
        }

        // GET: api/Notification/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(NotificationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _notificationService.GetByIdAsync(id);
            if (item is null) return NotFound($"Notification {id} not found.");
            return Ok(item);
        }

        // POST: api/Notification
        [HttpPost]
        [ProducesResponseType(typeof(NotificationDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] NotificationCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _notificationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.NotificationId }, created);
        }

        // PUT: api/Notification/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(NotificationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] NotificationUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // keep route/body ids consistent (optional)
            if (dto.NotificationId == Guid.Empty) dto.NotificationId = id;
            else if (dto.NotificationId != id) return BadRequest("Route id and body NotificationId mismatch.");

            var updated = await _notificationService.UpdateAsync(id, dto);
            if (updated is null) return NotFound($"Notification {id} not found.");

            return Ok(updated);
        }

        // PATCH: api/Notification/{id}/read?isRead=true
        [HttpPatch("{id:guid}/read")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkRead(Guid id, [FromQuery] bool isRead = true)
        {
            var ok = await _notificationService.MarkReadAsync(id, isRead);
            if (!ok) return NotFound($"Notification {id} not found.");
            return Ok(new { notificationId = id, isRead });
        }

        // PATCH: api/Notification/user/{userId}/read-all
        [HttpPatch("user/{userId:guid}/read-all")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAllRead(Guid userId)
        {
            var affected = await _notificationService.MarkAllReadAsync(userId);
            return Ok(new { userId, affected });
        }

        // GET: api/Notification/user/{userId}/unread-count
        [HttpGet("user/{userId:guid}/unread-count")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnreadCount(Guid userId)
        {
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { userId, unread = count });
        }

        // DELETE: api/Notification/{id}
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _notificationService.DeleteAsync(id);
            if (!ok) return NotFound($"Notification {id} not found.");
            return NoContent();
        }
    }
}
