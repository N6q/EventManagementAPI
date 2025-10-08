using AutoMapper;
using EventManagementAPI.DTOs.Report;
using EventManagementAPI.DTOs.External;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Services.Implementations
{
    /// <summary>
    /// Implements business logic for generating event reports.
    /// Combines event, attendee, and live weather data for a complete overview.
    /// </summary>
    public class EventReportService : IEventReportService
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IExternalWeatherService _weatherService;

        public EventReportService(
            IEventRepository eventRepo,
            IMapper mapper,
            IExternalWeatherService weatherService)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _weatherService = weatherService;
        }

        // ======================================================
        // 🔹 GENERATE SINGLE EVENT REPORT
        // ======================================================
        public async Task<EventReportDto?> GenerateReportAsync(int eventId)
        {
            var ev = await _eventRepo.GetWithAttendeesAsync(eventId);
            if (ev == null)
                return null;

            var report = _mapper.Map<EventReportDto>(ev);
            report.AttendeeCount = ev.Attendees.Count;
            report.GeneratedAt = DateTime.UtcNow;

            // 🔸 Add weather for event location
            try
            {
                report.Weather = await _weatherService.GetWeatherAsync(ev.Location)
                    ?? new WeatherInfoDto("No data", null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Weather API error for '{ev.Location}': {ex.Message}");
                report.Weather = new WeatherInfoDto("Unavailable", null, null);
            }

            return report;
        }

        // ======================================================
        // 🔹 GENERATE UPCOMING REPORTS (Next 30 Days)
        // ======================================================
        public async Task<IEnumerable<EventReportDto>> GetUpcomingReportsAsync()
        {
            var now = DateTime.UtcNow;
            var events = await _eventRepo.Query()
                .Where(e => e.Date >= now && e.Date <= now.AddDays(30))
                .Include(e => e.Attendees)
                .ToListAsync();

            var reports = new List<EventReportDto>();

            foreach (var ev in events)
            {
                var report = _mapper.Map<EventReportDto>(ev);
                report.AttendeeCount = ev.Attendees.Count;
                report.GeneratedAt = DateTime.UtcNow;
                reports.Add(report);
            }

            return reports;
        }

        // ======================================================
        // 🔹 GET SINGLE REPORT WITH WEATHER
        // ======================================================
        public async Task<EventReportDto?> GetByIdWithWeatherAsync(int eventId)
        {
            var report = await GenerateReportAsync(eventId);
            return report;
        }

        // ======================================================
        // 🔹 GET UPCOMING REPORTS WITH WEATHER
        // ======================================================
        public async Task<IEnumerable<EventReportDto>> GetUpcomingWithWeatherAsync()
        {
            var events = await GetUpcomingReportsAsync();
            var list = new List<EventReportDto>();

            foreach (var report in events)
            {
                try
                {
                    var weather = await _weatherService.GetWeatherAsync(report.Location);
                    report.Weather = weather ?? new WeatherInfoDto("No data", null, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Failed to fetch weather for '{report.Location}': {ex.Message}");
                    report.Weather = new WeatherInfoDto("Unavailable", null, null);
                }

                list.Add(report);
            }

            return list;
        }
    }
}
