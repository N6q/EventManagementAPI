using EventManagementAPI.DTOs.External;

namespace EventManagementAPI.DTOs.Report
{
    /// <summary>
    /// Represents a summarized report for an event,
    /// including attendee count and optional weather data.
    /// </summary>
    public class EventReportDto
    {
        // ======================================================
        // 🔹 EVENT INFORMATION
        // ======================================================
        public int EventId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = default!;

        // ======================================================
        // 🔹 REPORT DATA
        // ======================================================
        public int AttendeeCount { get; set; }

        /// <summary>
        /// The date and time when this report was generated.
        /// </summary>
        public DateTime GeneratedAt { get; set; }

        // ======================================================
        // 🔹 WEATHER INFORMATION
        // ======================================================
        // Retrieved dynamically from the external weather API.
        // Provides temperature, time, and weather summary.
        public WeatherInfoDto? Weather { get; set; }
    }
}
