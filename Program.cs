using EventManagementAPI.Data;
using EventManagementAPI.Mapping;
using EventManagementAPI.Repositories.Implementations;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Services.Implementations;
using EventManagementAPI.Services.Interfaces;
using EventManagementAPI.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace EventManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ======================================================
            // 🔹 BUILD CONFIGURATION & LOGGING
            // ======================================================
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog for console and file logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            // ======================================================
            // 🔹 SERVICES REGISTRATION
            // ======================================================
            builder.Services.AddControllers();

            // Database Context (EF Core + Lazy Loading)
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            // Repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

            // Services
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IAttendeeService, AttendeeService>();
            builder.Services.AddScoped<IEventReportService, EventReportService>();

            // JWT options + token service
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

            // ======================================================
            // 🔹 EXTERNAL SERVICES (HTTP CLIENTS)
            // ======================================================
            // Register the weather API service for external requests
            builder.Services.AddHttpClient<IExternalWeatherService, ExternalWeatherService>();

            // ======================================================
            // 🔹 AUTHENTICATION (JWT)
            // ======================================================
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            // ======================================================
            // 🔹 SWAGGER
            // ======================================================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Event Management API",
                    Description = "RESTful API for managing events, attendees, and reports.",
                    Contact = new OpenApiContact
                    {
                        Name = "Samir Al-Bulushi",
                        Email = "albulushii.samir@gmail.com",
                        Url = new Uri("https://github.com/N6q?tab=repositories")
                    }
                });

                // JWT support in Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}' (without quotes)",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // ======================================================
            // 🔹 BUILD & MIDDLEWARE PIPELINE
            // ======================================================
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // ======================================================
            // 🔹 APPLICATION STARTUP
            // ======================================================
            try
            {
                Log.Information("🚀 EventManagementAPI starting up...");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "❌ EventManagementAPI failed to start correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
