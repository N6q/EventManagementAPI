using EventManagementAPI.DTOs.Report;
using EventManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementAPI.Controllers
{
    /// <summary>
    /// Generates analytical event reports combining attendee data and weather forecasts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IEventReportService _reportService;

        public ReportController(IEventReportService reportService)
        {
            _reportService = reportService;
        }

        // ======================================================
        // 🔹 UPCOMING EVENTS REPORT
        // ======================================================
        /// <summary>
        /// Retrieves all upcoming events (next 30 days) including:
        /// - Attendee count
        /// - Weather forecast (via external API)
        /// </summary>
        [HttpGet("upcoming")]
        [ProducesResponseType(typeof(IEnumerable<EventReportDto>), 200)]
        [SwaggerResponse(200, "Upcoming events with attendee counts and weather info.", typeof(IEnumerable<EventReportDto>))]
        public async Task<ActionResult<IEnumerable<EventReportDto>>> GetUpcoming()
        {
            var reports = await _reportService.GetUpcomingWithWeatherAsync();
            return Ok(reports);
        }

        // ======================================================
        // 🔹 SINGLE EVENT REPORT
        // ======================================================
        /// <summary>
        /// Generates a report for a specific event, combining attendee data and weather info.
        /// </summary>
        [HttpGet("{eventId}")]
        [ProducesResponseType(typeof(EventReportDto), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Single event report generated successfully.", typeof(EventReportDto))]
        [SwaggerResponse(404, "Event not found.")]
        public async Task<ActionResult<EventReportDto>> GetReportById(int eventId)
        {
            var report = await _reportService.GetByIdWithWeatherAsync(eventId);
            if (report == null)
                return NotFound("Event not found or cannot generate report.");

            return Ok(report);
        }
    }
}
