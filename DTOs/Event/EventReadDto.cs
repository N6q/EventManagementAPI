// ======================================================
// 🔹 EVENT READ DTO
// ======================================================
// Represents event data returned to the client.
// Used in GET requests for event listing or details.
// ======================================================

namespace EventManagementAPI.DTOs.Event
{
    public class EventReadDto
    {
        public int EventId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = default!;
        public int MaxAttendees { get; set; }
    }
}
