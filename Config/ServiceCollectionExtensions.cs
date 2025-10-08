using EventManagementAPI.Data;
using EventManagementAPI.Mapping;
using EventManagementAPI.Repositories.Implementations;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Services.Implementations;
using EventManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace EventManagementAPI.Config
{
    /// <summary>
    /// Centralized extension methods for registering all dependencies and configurations.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        // ======================================================
        // 🔹 REGISTER ALL SERVICES
        // ======================================================
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // ------------------------------------------
            // Database
            // ------------------------------------------
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("Default")));


            // ------------------------------------------
            // Repositories
            // ------------------------------------------
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IAttendeeRepository, AttendeeRepository>();

            // ------------------------------------------
            // Services
            // ------------------------------------------
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IAttendeeService, AttendeeService>();
            services.AddScoped<IEventReportService, EventReportService>();

            // ------------------------------------------
            // AutoMapper
            // ------------------------------------------
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // ------------------------------------------
            // JWT Authentication
            // ------------------------------------------
            services.ConfigureJwtAuthentication(config);



            return services;
        }

        // ======================================================
        // 🔹 ADD LOGGING
        // ======================================================
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration)
                  .Enrich.FromLogContext());
        }
    }
}
