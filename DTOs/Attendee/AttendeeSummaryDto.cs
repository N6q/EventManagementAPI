namespace EventManagementAPI.DTOs.Attendee
{
    /// <summary>
    /// Represents minimal attendee information for listings.
    /// </summary>
    public class AttendeeSummaryDto
    {
        // ======================================================
        // 🔹 SUMMARY FIELDS
        // ======================================================
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
