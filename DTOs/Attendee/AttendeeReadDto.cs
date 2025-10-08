// ======================================================
// 🔹 ATTENDEE READ DTO
// ======================================================
// Represents attendee data returned to the client.
// Used in GET requests for event attendees.
// ======================================================

namespace EventManagementAPI.DTOs.Attendee
{
    public class AttendeeReadDto
    {
        public int AttendeeId { get; set; }
        public int EventId { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
