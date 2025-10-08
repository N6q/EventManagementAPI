using EventManagementAPI.DTOs.Report;

namespace EventManagementAPI.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for generating and retrieving
    /// event reports with attendee statistics and optional weather data.
    /// </summary>
    public interface IEventReportService
    {
        // ======================================================
        // 🔹 GENERATE SINGLE EVENT REPORT
        // ======================================================
        /// <summary>
        /// Generates a detailed report for a specific event,
        /// including attendee count and generated timestamp.
        /// </summary>
        /// <param name="eventId">The unique ID of the event.</param>
        /// <returns>A complete EventReportDto or null if not found.</returns>
        Task<EventReportDto?> GenerateReportAsync(int eventId);

        // ======================================================
        // 🔹 GENERATE UPCOMING REPORTS
        // ======================================================
        /// <summary>
        /// Generates reports for all events occurring within
        /// the next 30 days (without weather data).
        /// </summary>
        /// <returns>A collection of EventReportDto objects.</returns>
        Task<IEnumerable<EventReportDto>> GetUpcomingReportsAsync();

        // ======================================================
        // 🔹 GET SINGLE REPORT WITH WEATHER
        // ======================================================
        /// <summary>
        /// Generates a report for a specific event and attaches
        /// live weather data for the event’s location.
        /// </summary>
        /// <param name="eventId">The unique ID of the event.</param>
        /// <returns>An EventReportDto enriched with weather information.</returns>
        Task<EventReportDto?> GetByIdWithWeatherAsync(int eventId);

        // ======================================================
        // 🔹 GET UPCOMING REPORTS WITH WEATHER
        // ======================================================
        /// <summary>
        /// Retrieves all upcoming event reports (next 30 days)
        /// including attendee count and weather forecast data.
        /// </summary>
        /// <returns>A collection of EventReportDto objects with weather info.</returns>
        Task<IEnumerable<EventReportDto>> GetUpcomingWithWeatherAsync();
    }
}
