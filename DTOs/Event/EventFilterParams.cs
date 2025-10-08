// ======================================================
// 🔹 EVENT FILTER PARAMETERS DTO
// ======================================================
// Used to filter and sort event listings using query parameters.
// Example:
//   /events?location=Muscat&sortBy=date&desc=true
// ======================================================

namespace EventManagementAPI.DTOs.Event
{
    public class EventFilterParams
    {
        public string? Location { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? SortBy { get; set; } // date | title | attendees
        public bool Desc { get; set; } = false;
    }
}
