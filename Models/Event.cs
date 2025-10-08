// ======================================================
// 🔹 EVENT MODEL
// ======================================================
// Represents an event that users can register for.
// Each event can have multiple attendees.
// ======================================================

using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models
{
    public class Event
    {
        // ======================================================
        // 🔸 PRIMARY KEY
        // ======================================================
        // The unique identifier for each event.
        // [Key] explicitly marks this as the table's primary key.
        // Even though EF Core can infer it automatically,
        // we keep it for clarity and readability.
        // ======================================================
        [Key]
        public int EventId { get; set; }

        // ======================================================
        // 🔸 EVENT INFORMATION
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

        // ======================================================
        // 🔸 RELATIONSHIPS
        // ======================================================
        // Each event can have multiple attendees.
        // "virtual" enables EF Core lazy loading proxies.
        // When accessed, EF will automatically fetch the
        // related Attendees collection from the database.
        // ======================================================
        public virtual List<Attendee> Attendees { get; set; } = new();
    }
}
