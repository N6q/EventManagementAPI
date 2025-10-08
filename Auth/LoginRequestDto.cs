// ======================================================
// 🔹 LOGIN REQUEST DTO
// ======================================================
// Represents incoming login credentials from the client.
// Used in POST /api/auth/login.
// ======================================================
using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
