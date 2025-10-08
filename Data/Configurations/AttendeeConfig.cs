// ======================================================
// 🔹 ATTENDEE CONFIGURATION
// ======================================================
// Defines database constraints and relationships for Attendee entity
// using EF Core Fluent API.
// Ensures email uniqueness per event and proper foreign key setup.
// ======================================================

using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementAPI.Data.Configurations
{
    public class AttendeeConfig : IEntityTypeConfiguration<Attendee>
    {
        public void Configure(EntityTypeBuilder<Attendee> builder)
        {
            // ======================================================
            // 🔸 PRIMARY KEY
            // ======================================================
            builder.HasKey(a => a.AttendeeId);

            // ======================================================
            // 🔸 PROPERTY RULES
            // ======================================================
            builder.Property(a => a.FullName)
                   .IsRequired()
                   .HasMaxLength(80);

            builder.Property(a => a.Email)
                   .IsRequired();

            builder.Property(a => a.RegisteredAt)
                   .HasDefaultValueSql("GETUTCDATE()"); // ✅ Server-side default

            // ======================================================
            // 🔸 RELATIONSHIP (Many Attendees → 1 Event)
            // ======================================================
            builder.HasOne(a => a.Event)
                   .WithMany(e => e.Attendees)
                   .HasForeignKey(a => a.EventId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ======================================================
            // 🔸 UNIQUE CONSTRAINT
            // ======================================================
            // Prevents duplicate registration for the same event using same email.
            // ======================================================
            builder.HasIndex(a => new { a.Email, a.EventId })
                   .IsUnique()
                   .HasDatabaseName("IX_Attendee_Email_EventId");
        }
    }
}
