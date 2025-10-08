// ======================================================
// 🔹 JWT TOKEN SERVICE
// ======================================================
// Responsible for generating JWT tokens with user claims.
// Uses Microsoft.IdentityModel.Tokens for signing and validation.
// ======================================================
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EventManagementAPI.Auth
{
    public interface IJwtTokenService
    {
        LoginResponseDto GenerateToken(string username, string role);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwt;

        // Inject settings via IOptions pattern
        public JwtTokenService(IOptions<JwtSettings> options)
        {
            _jwt = options.Value;
        }

        // ======================================================
        // 🔹 GENERATE JWT TOKEN
        // ======================================================
        // Accepts username + role and issues a signed JWT token
        // with a limited expiration time defined in appsettings.json.
        // ======================================================
        public LoginResponseDto GenerateToken(string username, string role)
        {
            // Define token claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create signing credentials using secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token expiration
            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes);

            // Build the token
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            // Return token + expiry info
            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }
}
