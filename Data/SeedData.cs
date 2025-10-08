// ======================================================
// 🔹 DATABASE SEED DATA
// ======================================================
// Seeds initial sample records into the database for demo purposes.
// Ensures at least two events exist when the app is first run.
// ======================================================

using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Data
{
    public static class SeedData
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            // ======================================================
            // 🔸 SEED EVENTS
            // ======================================================
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    EventId = 1,
                    Title = "Oman Tech Innovation Summit",
                    Description = "Exploring AI, Cloud, and IoT trends shaping Oman’s digital future.",
                    Date = DateTime.UtcNow.AddDays(14),
                    Location = "Muscat",
                    MaxAttendees = 200
                },
                new Event
                {
                    EventId = 2,
                    Title = "Data Science Bootcamp 2025",
                    Description = "Intensive hands-on training for data analysis and ML fundamentals.",
                    Date = DateTime.UtcNow.AddDays(25),
                    Location = "Sohar",
                    MaxAttendees = 150
                }
            );

            // ======================================================
            // 🔸 NOTE:
            // Attendees are not seeded here.
            // They will be dynamically added when users register.
            // ======================================================
        }
    }
}
