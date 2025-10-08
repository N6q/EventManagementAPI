// ======================================================
// 🔹 EVENT CREATE DTO
// ======================================================
// Represents the data needed to create a new event.
// Used in POST /events endpoint.
// ======================================================

using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.DTOs.Event
{
    public class EventCreateDto
    {
        // ======================================================
        // 🔸 EVENT DETAILS
        // ======================================================
        [Required, MaxLength(100)]
        public string Title { get; set; } = default!;

        [MaxLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(100)]
        public string Location { get; set; } = default!;

        [Range(10, 500)]
        public int MaxAttendees { get; set; }
    }
}
