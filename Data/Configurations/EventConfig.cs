// ======================================================
// 🔹 EVENT CONFIGURATION
// ======================================================
// Defines the database configuration for the Event entity
// using EF Core Fluent API.
// This includes constraints, relationships, and indexes.
// ======================================================

using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementAPI.Data.Configurations
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // ======================================================
            // 🔸 PRIMARY KEY
            // ======================================================
            builder.HasKey(e => e.EventId);

            // ======================================================
            // 🔸 PROPERTY RULES
            // ======================================================
            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Description)
                   .HasMaxLength(300);

            builder.Property(e => e.Date)
                   .IsRequired();

            builder.Property(e => e.Location)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.MaxAttendees)
                   .IsRequired()
                   .HasDefaultValue(10); // ✅ Default minimum

            // ======================================================
            // 🔸 RELATIONSHIP (1 Event → Many Attendees)
            // ======================================================
            builder.HasMany(e => e.Attendees)
                   .WithOne(a => a.Event)
                   .HasForeignKey(a => a.EventId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete all attendees if event is deleted

            // ======================================================
            // 🔸 INDEXES
            // ======================================================
            builder.HasIndex(e => new { e.Date, e.Location })
                   .HasDatabaseName("IX_Event_Date_Location");
        }
    }
}
