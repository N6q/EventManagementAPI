// ======================================================
// 🔹 EVENT WITH ATTENDEES DTO
// ======================================================
// Extends EventReadDto by including a list of attendees.
// Used in endpoints where event-attendee details are shown together.
// ======================================================

using EventManagementAPI.DTOs.Attendee;

namespace EventManagementAPI.DTOs.Event
{
    public class EventWithAttendeesDto : EventReadDto
    {
        public List<AttendeeReadDto> Attendees { get; set; } = new();
    }
}
