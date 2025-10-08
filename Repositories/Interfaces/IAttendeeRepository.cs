using EventManagementAPI.Models;

namespace EventManagementAPI.Repositories.Interfaces
{
    /// <summary>
    /// Defines attendee-specific operations and validations.
    /// Extends the generic repository to include event-based lookups.
    /// </summary>
    public interface IAttendeeRepository : IGenericRepository<Attendee>
    {
        // ======================================================
        // 🔹 ATTENDEE CUSTOM METHODS
        // ======================================================

        /// <summary>
        /// Retrieves all attendees for a given event.
        /// </summary>
        Task<IEnumerable<Attendee>> GetByEventIdAsync(int eventId);

        /// <summary>
        /// Checks if an email address is already registered in the system.
        /// </summary>
        Task<bool> IsEmailTakenAsync(string email);

        /// <summary>
        /// Checks if the user is already registered for a specific event.
        /// </summary>
        Task<bool> IsAlreadyRegisteredAsync(int eventId, string email);
    }
}
