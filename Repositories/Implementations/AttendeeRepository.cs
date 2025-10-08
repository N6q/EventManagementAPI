using EventManagementAPI.Models;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Repositories.Implementations
{
    /// <summary>
    /// Implements attendee-specific data logic and validations.
    /// </summary>
    public class AttendeeRepository : GenericRepository<Attendee>, IAttendeeRepository
    {
        private readonly AppDbContext _db;

        public AttendeeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        // ======================================================
        // 🔹 GET BY EVENT ID
        // ======================================================
        public async Task<IEnumerable<Attendee>> GetByEventIdAsync(int eventId)
            => await _db.Attendees
                        .Where(a => a.EventId == eventId)
                        .AsNoTracking()
                        .ToListAsync();

        // ======================================================
        // 🔹 CHECK EMAIL TAKEN
        // ======================================================
        public async Task<bool> IsEmailTakenAsync(string email)
            => await _db.Attendees.AnyAsync(a => a.Email == email);

        // ======================================================
        // 🔹 CHECK ALREADY REGISTERED
        // ======================================================
        public async Task<bool> IsAlreadyRegisteredAsync(int eventId, string email)
            => await _db.Attendees.AnyAsync(a => a.EventId == eventId && a.Email == email);
    }
}
