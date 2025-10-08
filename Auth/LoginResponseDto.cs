// ======================================================
// 🔹 LOGIN RESPONSE DTO
// ======================================================
// Returned to the client upon successful authentication.
// Contains the JWT token and its expiration time.
// ======================================================
namespace EventManagementAPI.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
    }
}
