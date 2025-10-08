using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventManagementAPI.Config
{
    /// <summary>
    /// Adds and configures JWT Bearer authentication for the API.
    /// </summary>
    public static class JwtConfig
    {
        // ======================================================
        // 🔹 CONFIGURE JWT
        // ======================================================
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
