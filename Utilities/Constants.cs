namespace EventManagementAPI.Utilities
{
    /// <summary>
    /// Contains global constants used throughout the system.
    /// Keeps configuration and validation values centralized.
    /// </summary>
    public static class Constants
    {
        // ======================================================
        // 🔹 EVENT VALIDATION CONSTANTS
        // ======================================================
        public const int MinAttendees = 10;
        public const int MaxAttendees = 500;
        public const int TitleMaxLength = 100;
        public const int DescriptionMaxLength = 300;
        public const int LocationMaxLength = 100;

        // ======================================================
        // 🔹 COMMON CACHE KEYS (for future use)
        // ======================================================
        public static class CacheKeys
        {
            public const string AllEvents = "cache_all_events";
            public static string EventById(int id) => $"cache_event_{id}";
            public static string AttendeesByEvent(int eventId) => $"cache_attendees_event_{eventId}";
        }

        // ======================================================
        // 🔹 ERROR MESSAGES
        // ======================================================
        public static class ErrorMessages
        {
            public const string EventNotFound = "Event not found.";
            public const string EventFull = "The event has reached its capacity limit.";
            public const string DuplicateRegistration = "Attendee already registered for this event.";
            public const string InvalidDate = "Event date cannot be in the past.";
        }
    }
}
