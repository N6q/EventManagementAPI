// ======================================================
// 🔹 EVENT WITH COUNT DTO
// ======================================================
// Extends EventReadDto by including the total number of attendees.
// Used in reports and summary views.
// ======================================================

namespace EventManagementAPI.DTOs.Event
{
    public class EventWithCountDto : EventReadDto
    {
        public int AttendeeCount { get; set; }
    }
}
