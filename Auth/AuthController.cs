// ======================================================
// 🔹 AUTH CONTROLLER
// ======================================================
// Handles authentication and JWT token issuance.
// For demo purposes, credentials are hardcoded.
// Replace with real user validation (e.g., DB lookup).
// ======================================================
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwt;

        public AuthController(IJwtTokenService jwt)
        {
            _jwt = jwt;
        }

        // ======================================================
        // 🔹 POST /api/auth/login
        // ======================================================
        // Accepts a username and password.
        // Returns a JWT token if credentials are valid.
        // ======================================================
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto dto)
        {
            // ⚠️ DEMO ONLY: Replace with real user validation later
            if (dto.Username == "admin" && dto.Password == "1234")
            {
                var token = _jwt.GenerateToken(dto.Username, "Admin");
                return Ok(token);
            }

            if (dto.Username == "user" && dto.Password == "1234")
            {
                var token = _jwt.GenerateToken(dto.Username, "User");
                return Ok(token);
            }

            // Invalid credentials
            return Unauthorized(new { message = "Invalid username or password." });
        }
    }
}
