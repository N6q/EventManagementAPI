using AutoMapper;
using EventManagementAPI.DTOs.Attendee;
using EventManagementAPI.Models;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Services.Interfaces;

namespace EventManagementAPI.Services.Implementations
{
    /// <summary>
    /// Implements business logic for managing attendees,
    /// including registration, capacity checks, and validation.
    /// </summary>
    public class AttendeeService : IAttendeeService
    {
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public AttendeeService(IAttendeeRepository attendeeRepo, IEventRepository eventRepo, IMapper mapper)
        {
            _attendeeRepo = attendeeRepo;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        // ======================================================
        // 🔹 REGISTER NEW ATTENDEE
        // ======================================================
        public async Task<AttendeeReadDto?> RegisterAsync(AttendeeCreateDto dto)
        {
            // ======================================================
            // 🔸 VALIDATE EVENT EXISTENCE
            // ======================================================
            var ev = await _eventRepo.GetWithAttendeesAsync(dto.EventId);
            if (ev == null)
                return null;

            // ======================================================
            // 🔸 BLOCK PAST EVENTS
            // ======================================================
            if (ev.Date < DateTime.UtcNow)
                return null;

            // ======================================================
            // 🔸 DUPLICATE REGISTRATION CHECK
            // ======================================================
            var alreadyRegistered = await _attendeeRepo.IsAlreadyRegisteredAsync(dto.EventId, dto.Email);
            if (alreadyRegistered)
                return null;

            // ======================================================
            // 🔸 CAPACITY CHECK
            // ======================================================
            var currentCount = ev.Attendees.Count;
            if (currentCount >= ev.MaxAttendees)
                return null;

            // ======================================================
            // 🔸 CREATE ATTENDEE
            // ======================================================
            var entity = _mapper.Map<Attendee>(dto);
            entity.RegisteredAt = DateTime.UtcNow;

            await _attendeeRepo.AddAsync(entity);
            return _mapper.Map<AttendeeReadDto>(entity);
        }

        // ======================================================
        // 🔹 GET ALL ATTENDEES FOR EVENT
        // ======================================================
        public async Task<IEnumerable<AttendeeReadDto>> GetByEventIdAsync(int eventId)
        {
            var attendees = await _attendeeRepo.GetByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<AttendeeReadDto>>(attendees);
        }

        // ======================================================
        // 🔹 GET ATTENDEE BY ID
        // ======================================================
        public async Task<AttendeeReadDto?> GetByIdAsync(int attendeeId)
        {
            var attendee = await _attendeeRepo.GetByIdAsync(attendeeId);
            return attendee == null ? null : _mapper.Map<AttendeeReadDto>(attendee);
        }

        // ======================================================
        // 🔹 VALIDATION
        // ======================================================
        public async Task<bool> IsEmailTakenAsync(string email)
            => await _attendeeRepo.IsEmailTakenAsync(email);

        public async Task<bool> IsAlreadyRegisteredAsync(int eventId, string email)
            => await _attendeeRepo.IsAlreadyRegisteredAsync(eventId, email);

        // ======================================================
        // 🔹 DELETE ATTENDEE
        // ======================================================
        public async Task<bool> DeleteAsync(int id)
        {
            var attendee = await _attendeeRepo.GetByIdAsync(id);
            if (attendee == null)
                return false;

            await _attendeeRepo.DeleteAsync(attendee);
            return true;
        }
    }
}
