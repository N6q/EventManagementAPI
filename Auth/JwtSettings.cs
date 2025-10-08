// ======================================================
// 🔹 JWT SETTINGS MODEL
// ======================================================
// Holds all JWT configuration options loaded from appsettings.json.
// This model is bound automatically via:
// builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
namespace EventManagementAPI.Auth
{
    public class JwtSettings
    {
        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpireMinutes { get; set; } = 60;
    }
}
