using EventManagementAPI.DTOs.Attendee;

namespace EventManagementAPI.Services.Interfaces
{
    /// <summary>
    /// Defines the service contract for managing attendees.
    /// Handles registration, validation, and data retrieval
    /// for event participation.
    /// </summary>
    public interface IAttendeeService
    {
        // ======================================================
        // 🔹 REGISTRATION
        // ======================================================

        /// <summary>
        /// Registers a new attendee for a specific event.
        /// </summary>
        Task<AttendeeReadDto?> RegisterAsync(AttendeeCreateDto dto);

        // ======================================================
        // 🔹 RETRIEVAL
        // ======================================================

        /// <summary>
        /// Retrieves all attendees for a specific event.
        /// </summary>
        Task<IEnumerable<AttendeeReadDto>> GetByEventIdAsync(int eventId);

        /// <summary>
        /// Retrieves details of a single attendee by ID.
        /// </summary>
        Task<AttendeeReadDto?> GetByIdAsync(int attendeeId);

        // ======================================================
        // 🔹 VALIDATION
        // ======================================================

        /// <summary>
        /// Checks if an email is already used by another attendee.
        /// </summary>
        Task<bool> IsEmailTakenAsync(string email);

        /// <summary>
        /// Checks if the user is already registered for a specific event.
        /// </summary>
        Task<bool> IsAlreadyRegisteredAsync(int eventId, string email);

        // ======================================================
        // 🔹 DELETE
        // ======================================================

        /// <summary>
        /// Removes an attendee registration.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
