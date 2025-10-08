using EventManagementAPI.DTOs.Event;

namespace EventManagementAPI.Services.Interfaces
{
    /// <summary>
    /// Defines the service contract for managing events.
    /// Includes CRUD operations and business logic such as
    /// attendee tracking, validation, and event availability.
    /// </summary>
    public interface IEventService
    {
        // ======================================================
        // 🔹 CREATE / UPDATE / DELETE
        // ======================================================

        /// <summary>
        /// Creates a new event in the system.
        /// </summary>
        Task<EventReadDto> CreateAsync(EventCreateDto dto);

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        Task<bool> UpdateAsync(int id, EventCreateDto dto);

        /// <summary>
        /// Deletes an event and its related data.
        /// </summary>
        Task<bool> DeleteAsync(int id);

        // ======================================================
        // 🔹 READ OPERATIONS
        // ======================================================

        /// <summary>
        /// Retrieves all events with optional attendee count.
        /// </summary>
        Task<IEnumerable<EventWithCountDto>> GetAllAsync();

        /// <summary>
        /// Retrieves event details with attendee list.
        /// </summary>
        Task<EventWithAttendeesDto?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves upcoming events only.
        /// </summary>
        Task<IEnumerable<EventWithCountDto>> GetUpcomingAsync();

        // ======================================================
        // 🔹 VALIDATION / UTILITIES
        // ======================================================

        /// <summary>
        /// Checks whether an event with the given ID exists.
        /// </summary>
        Task<bool> ExistsAsync(int id);
    }
}
