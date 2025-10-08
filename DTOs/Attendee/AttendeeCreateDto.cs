// ======================================================
// 🔹 ATTENDEE CREATE DTO
// ======================================================
// Represents the data required to register an attendee for an event.
// Used in POST /attendees endpoint.
// ======================================================

using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.DTOs.Attendee
{
    public class AttendeeCreateDto
    {
        // ======================================================
        // 🔸 PERSONAL DETAILS
        // ======================================================
        [Required, MaxLength(80)]
        public string FullName { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        public string? Phone { get; set; }

        // ======================================================
        // 🔸 EVENT LINK
        // ======================================================
        [Required]
        public int EventId { get; set; }
    }
}
