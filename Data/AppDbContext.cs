// ======================================================
// 🔹 APPLICATION DB CONTEXT
// ======================================================
// Defines the database structure using Entity Framework Core.
// Includes DbSets for Events and Attendees.
// Enables lazy loading and applies configurations + seeding.
// ======================================================

using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EventManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        // ======================================================
        // 🔸 CONSTRUCTOR
        // ======================================================
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ======================================================
        // 🔸 DBSETS (TABLES)
        // ======================================================
        // Each DbSet represents a table in the database.
        // ======================================================
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Attendee> Attendees => Set<Attendee>();

        // ======================================================
        // 🔸 MODEL CONFIGURATION
        // ======================================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Automatically apply all configurations
            // from the current assembly (Data/Configurations folder).
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply seeding for initial demo data.
            SeedData.Apply(modelBuilder);
        }
    }
}
