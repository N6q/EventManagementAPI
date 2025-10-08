// ======================================================
// 🔹 ATTENDEE MODEL
// ======================================================
// Represents a person who registers for an event.
// Each attendee belongs to a single event (Many-to-One).
// ======================================================

using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models
{
    public class Attendee
    {
        // ======================================================
        // 🔸 PRIMARY KEY
        // ======================================================
        // The unique identifier for each attendee.
        // ======================================================
        [Key]
        public int AttendeeId { get; set; }

        // ======================================================
        // 🔸 PERSONAL INFORMATION
        // ======================================================
        [Required, MaxLength(80)]
        public string FullName { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        public string? Phone { get; set; }

        // ======================================================
        // 🔸 REGISTRATION DETAILS
        // ======================================================
        // Automatically sets the current UTC time when the attendee
        // is created (acts as default registration timestamp).
        // ======================================================
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        // ======================================================
        // 🔸 RELATIONSHIP TO EVENT
        // ======================================================
        // The foreign key that links the attendee to its event.
        // "virtual" allows EF Core lazy loading to fetch the event
        // only when it is accessed.
        // ======================================================
        [Required]
        public int EventId { get; set; }

        public virtual Event? Event { get; set; }
    }
}
