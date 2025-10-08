using EventManagementAPI.DTOs.Attendee;
using EventManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementAPI.Controllers
{
    /// <summary>
    /// Handles attendee registration and event attendance lookups.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeeController : ControllerBase
    {
        private readonly IAttendeeService _service;

        public AttendeeController(IAttendeeService service)
        {
            _service = service;
        }

        // ======================================================
        // 🔹 GET ATTENDEES BY EVENT
        // ======================================================
        /// <summary>
        /// Retrieves all attendees for a given event (lazy loaded).
        /// </summary>
        [HttpGet("event/{eventId:int}")]
        [ProducesResponseType(typeof(IEnumerable<AttendeeReadDto>), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Attendees retrieved successfully.", typeof(IEnumerable<AttendeeReadDto>))]
        [SwaggerResponse(404, "No attendees found for this event.")]
        public async Task<ActionResult<IEnumerable<AttendeeReadDto>>> GetByEvent(int eventId)
        {
            var attendees = await _service.GetByEventIdAsync(eventId);
            if (!attendees.Any()) return NotFound();
            return Ok(attendees);
        }

        // ======================================================
        // 🔹 REGISTER ATTENDEE
        // ======================================================
        /// <summary>
        /// Registers a new attendee for a specified event.
        /// Ensures capacity and duplicate email validation.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(AttendeeReadDto), 201)]
        [ProducesResponseType(400)]
        [SwaggerResponse(201, "Attendee registered successfully.", typeof(AttendeeReadDto))]
        [SwaggerResponse(400, "Invalid input or registration could not be completed.")]
        public async Task<ActionResult<AttendeeReadDto>> Register([FromBody] AttendeeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.RegisterAsync(dto);

            // ======================================================
            // 🔸 VALIDATION HANDLING
            // ======================================================
            if (result == null)
                return BadRequest("Registration failed: event not found, already registered, full, or closed.");

            return CreatedAtAction(nameof(GetByEvent), new { eventId = result.EventId }, result);
        }
    }
}
