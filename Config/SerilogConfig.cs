using Serilog;
using Microsoft.Extensions.Configuration;

namespace EventManagementAPI.Config
{
    /// <summary>
    /// Initializes Serilog configuration for structured logging.
    /// </summary>
    public static class SerilogConfig
    {
        // ======================================================
        // 🔹 CONFIGURE SERILOG
        // ======================================================
        public static void InitializeSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration)
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));
        }
    }
}
