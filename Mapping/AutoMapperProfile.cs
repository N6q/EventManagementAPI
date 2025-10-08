// ======================================================
// 🔹 AUTOMAPPER PROFILE
// ======================================================
// Defines all mappings between domain models and DTOs.
// Ensures clean separation between database entities
// and API data contracts.
// ======================================================

using AutoMapper;
using EventManagementAPI.Models;
using EventManagementAPI.DTOs.Event;
using EventManagementAPI.DTOs.Attendee;
using EventManagementAPI.DTOs.Report;

namespace EventManagementAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ======================================================
            // 🔹 EVENT MAPPINGS
            // ======================================================
            // Maps between Event entity and various Event DTOs.
            // ======================================================
            CreateMap<Event, EventReadDto>();

            CreateMap<Event, EventWithAttendeesDto>()
                .ForMember(dest => dest.Attendees, opt => opt.MapFrom(src => src.Attendees));

            CreateMap<Event, EventWithCountDto>()
                .ForMember(dest => dest.AttendeeCount, opt => opt.MapFrom(src => src.Attendees.Count));

            CreateMap<EventCreateDto, Event>();

            // ======================================================
            // 🔹 ATTENDEE MAPPINGS
            // ======================================================
            // Maps between Attendee entity and DTOs.
            // ======================================================
            CreateMap<Attendee, AttendeeReadDto>();

            CreateMap<AttendeeCreateDto, Attendee>()
                .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ======================================================
            // 🔹 REPORT MAPPINGS
            // ======================================================
            // Combines event + attendee count for reporting layer.
            // ======================================================
            CreateMap<Event, EventReportDto>()
                .ForMember(dest => dest.AttendeeCount, opt => opt.MapFrom(src => src.Attendees.Count));
        }
    }
}
