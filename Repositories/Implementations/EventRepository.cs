using EventManagementAPI.Models;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Repositories.Implementations
{
    /// <summary>
    /// Implements event-specific data operations such as retrieving
    /// events with attendees or upcoming event listings.
    /// </summary>
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly AppDbContext _db;

        public EventRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        // ======================================================
        // 🔹 GET EVENT WITH ATTENDEES
        // ======================================================
        public async Task<Event?> GetWithAttendeesAsync(int eventId)
            => await _db.Events
                        .Include(e => e.Attendees)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.EventId == eventId);

        // ======================================================
        // 🔹 GET UPCOMING EVENTS
        // ======================================================
        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
            => await _db.Events
                        .Where(e => e.Date >= DateTime.UtcNow)
                        .OrderBy(e => e.Date)
                        .AsNoTracking()
                        .ToListAsync();
    }
}
