using EventManagementAPI.DTOs.Event;
using EventManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementAPI.Controllers
{
    /// <summary>
    /// Manages event creation, updates, deletion, and retrieval.
    /// Handles all event-related API operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;

        public EventsController(IEventService service)
        {
            _service = service;
        }

        // ======================================================
        // 🔹 GET ALL EVENTS
        // ======================================================
        /// <summary>
        /// Retrieves all events with attendee counts.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<EventWithCountDto>), 200)]
        [SwaggerResponse(200, "All events retrieved successfully.", typeof(IEnumerable<EventWithCountDto>))]
        public async Task<ActionResult<IEnumerable<EventWithCountDto>>> GetAll()
        {
            var events = await _service.GetAllAsync();
            return Ok(events);
        }

        // ======================================================
        // 🔹 GET SINGLE EVENT BY ID
        // ======================================================
        /// <summary>
        /// Retrieves a specific event with its attendees.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(EventWithAttendeesDto), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Event found and returned successfully.", typeof(EventWithAttendeesDto))]
        [SwaggerResponse(404, "Event not found.")]
        public async Task<ActionResult<EventWithAttendeesDto>> GetById(int id)
        {
            var ev = await _service.GetByIdAsync(id);
            if (ev == null)
                return NotFound("Event not found.");

            return Ok(ev);
        }

        // ======================================================
        // 🔹 GET UPCOMING EVENTS
        // ======================================================
        /// <summary>
        /// Retrieves all upcoming events (next 30 days).
        /// </summary>
        [HttpGet("upcoming")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<EventWithCountDto>), 200)]
        [SwaggerResponse(200, "Upcoming events retrieved successfully.", typeof(IEnumerable<EventWithCountDto>))]
        public async Task<ActionResult<IEnumerable<EventWithCountDto>>> GetUpcoming()
        {
            var upcoming = await _service.GetUpcomingAsync();
            return Ok(upcoming);
        }

        // ======================================================
        // 🔹 CREATE EVENT
        // ======================================================
        /// <summary>
        /// Creates a new event in the system.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(EventReadDto), 201)]
        [ProducesResponseType(400)]
        [SwaggerResponse(201, "Event created successfully.", typeof(EventReadDto))]
        [SwaggerResponse(400, "Invalid event data.")]
        public async Task<ActionResult<EventReadDto>> Create([FromBody] EventCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.EventId }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ======================================================
        // 🔹 UPDATE EVENT
        // ======================================================
        /// <summary>
        /// Updates an existing event’s details.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerResponse(204, "Event updated successfully.")]
        [SwaggerResponse(400, "Invalid event data.")]
        [SwaggerResponse(404, "Event not found.")]
        public async Task<IActionResult> Update(int id, [FromBody] EventCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound("Event not found or cannot be updated.");

            return NoContent();
        }

        // ======================================================
        // 🔹 DELETE EVENT
        // ======================================================
        /// <summary>
        /// Deletes an existing event and its attendees.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [SwaggerResponse(204, "Event deleted successfully.")]
        [SwaggerResponse(404, "Event not found.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound("Event not found or already deleted.");

            return NoContent();
        }
    }
}
