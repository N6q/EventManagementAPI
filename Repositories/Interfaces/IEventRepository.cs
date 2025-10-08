using EventManagementAPI.Models;

namespace EventManagementAPI.Repositories.Interfaces
{
    /// <summary>
    /// Defines custom data operations for the Event entity.
    /// Extends the generic repository with event-specific logic.
    /// </summary>
    public interface IEventRepository : IGenericRepository<Event>
    {
        // ======================================================
        // 🔹 EVENT CUSTOM METHODS
        // ======================================================

        /// <summary>
        /// Retrieves an event and its attendees (eager loading).
        /// </summary>
        Task<Event?> GetWithAttendeesAsync(int eventId);

        /// <summary>
        /// Retrieves all upcoming events based on their date.
        /// </summary>
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    }
}
