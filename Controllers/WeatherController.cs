using EventManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementAPI.Controllers
{
    /// <summary>
    /// Provides access to external weather forecast data for events.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IExternalWeatherService _weatherService;

        public WeatherController(IExternalWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // ======================================================
        // 🔹 GET WEATHER FORECAST
        // ======================================================
        /// <summary>
        /// Retrieves the weather forecast for a specific city.
        /// </summary>
        /// <param name="city">The city name for which weather data is requested.</param>
        /// <returns>Weather summary and temperature information.</returns>
        [HttpGet("forecast")]
        [SwaggerResponse(200, "Weather data retrieved successfully.")]
        [SwaggerResponse(404, "City not found or API unavailable.")]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            var weather = await _weatherService.GetWeatherAsync(city);

            if (weather == null)
                return NotFound("No weather data available.");

            return Ok(weather);
        }
    }
}
