using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventManagementAPI.Middleware
{
    /// <summary>
    /// Custom middleware for JWT authentication and token validation.
    /// Validates the Authorization header and sets the user context.
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly IConfiguration _config;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger, IConfiguration config)
        {
            _next = next;
            _logger = logger;
            _config = config;
        }

        // ======================================================
        // 🔹 INVOKE PIPELINE
        // ======================================================
        /// <summary>
        /// Reads and validates the JWT token from the Authorization header.
        /// If valid, attaches the claims principal to the HttpContext.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
                AttachUserToContext(context, token);

            // ======================================================
            // 🔹 DYNAMIC AUTH CHECK
            // ======================================================
            // If the current endpoint has [Authorize] attribute, enforce JWT.
            var endpoint = context.GetEndpoint();
            var requiresAuth = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null;

            if (requiresAuth)
            {
                if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Missing or invalid token");
                    return;
                }
            }

            await _next(context);
        }

        // ======================================================
        // 🔹 TOKEN VALIDATION
        // ======================================================
        /// <summary>
        /// Validates the JWT token and attaches claims to the HttpContext if successful.
        /// </summary>
        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToList();
                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("JWT token expired.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"JWT validation failed: {ex.Message}");
            }
        }
    }
}
