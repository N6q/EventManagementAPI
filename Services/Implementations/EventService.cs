using AutoMapper;
using EventManagementAPI.DTOs.Event;
using EventManagementAPI.Models;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Services.Interfaces;

namespace EventManagementAPI.Services.Implementations
{
    /// <summary>
    /// Implements business logic for managing events,
    /// including creation, update, deletion, and validation rules.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        // ======================================================
        // 🔹 CREATE EVENT
        // ======================================================
        /// <summary>
        /// Creates a new event in the system.
        /// </summary>
        public async Task<EventReadDto> CreateAsync(EventCreateDto dto)
        {
            // ======================================================
            // 🔸 VALIDATION
            // ======================================================
            if (dto.Date < DateTime.UtcNow)
                throw new ArgumentException("Event date cannot be in the past.");

            if (dto.MaxAttendees <= 0)
                throw new ArgumentException("MaxAttendees must be greater than zero.");

            // ======================================================
            // 🔸 CREATE ENTITY
            // ======================================================
            var entity = _mapper.Map<Event>(dto);
            await _eventRepo.AddAsync(entity);
            return _mapper.Map<EventReadDto>(entity);
        }

        // ======================================================
        // 🔹 UPDATE EVENT
        // ======================================================
        /// <summary>
        /// Updates an existing event's details.
        /// </summary>
        public async Task<bool> UpdateAsync(int id, EventCreateDto dto)
        {
            var existing = await _eventRepo.GetByIdAsync(id);
            if (existing == null)
                return false;

            // Block updates for past events
            if (existing.Date < DateTime.UtcNow)
                return false;

            // Prevent invalid updates
            if (dto.MaxAttendees < existing.Attendees.Count)
                return false;

            _mapper.Map(dto, existing);
            await _eventRepo.UpdateAsync(existing);
            return true;
        }

        // ======================================================
        // 🔹 DELETE EVENT
        // ======================================================
        /// <summary>
        /// Deletes an event and its related attendees.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _eventRepo.GetWithAttendeesAsync(id);
            if (existing == null)
                return false;

            await _eventRepo.DeleteAsync(existing);
            return true;
        }

        // ======================================================
        // 🔹 GET ALL EVENTS
        // ======================================================
        /// <summary>
        /// Retrieves all events ordered by date (ascending).
        /// Includes attendee counts.
        /// </summary>
        public async Task<IEnumerable<EventWithCountDto>> GetAllAsync()
        {
            var events = await _eventRepo.GetAllAsync(includeProperties: "Attendees");
            var ordered = events.OrderBy(e => e.Date).ToList();
            return _mapper.Map<IEnumerable<EventWithCountDto>>(ordered);
        }

        // ======================================================
        // 🔹 GET EVENT BY ID (WITH ATTENDEES)
        // ======================================================
        /// <summary>
        /// Retrieves event details with a list of attendees.
        /// </summary>
        public async Task<EventWithAttendeesDto?> GetByIdAsync(int id)
        {
            var entity = await _eventRepo.GetWithAttendeesAsync(id);
            return entity == null ? null : _mapper.Map<EventWithAttendeesDto>(entity);
        }

        // ======================================================
        // 🔹 GET UPCOMING EVENTS
        // ======================================================
        /// <summary>
        /// Retrieves all upcoming events (next 30 days).
        /// </summary>
        public async Task<IEnumerable<EventWithCountDto>> GetUpcomingAsync()
        {
            var upcoming = await _eventRepo.GetUpcomingEventsAsync();
            var ordered = upcoming.OrderBy(e => e.Date).ToList();
            return _mapper.Map<IEnumerable<EventWithCountDto>>(ordered);
        }

        // ======================================================
        // 🔹 CHECK EXISTENCE
        // ======================================================
        /// <summary>
        /// Checks if an event with the given ID exists.
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
            => await _eventRepo.ExistsAsync(e => e.EventId == id);
    }
}
