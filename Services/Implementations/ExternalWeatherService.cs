using System.Net.Http.Json;
using EventManagementAPI.DTOs.External;
using EventManagementAPI.Services.Interfaces;

namespace EventManagementAPI.Services.Implementations
{
    /// <summary>
    /// Retrieves weather forecast data from an external API (Open-Meteo).
    /// </summary>
    public class ExternalWeatherService : IExternalWeatherService
    {
        private readonly HttpClient _httpClient;

        public ExternalWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ======================================================
        // 🔹 GET WEATHER FORECAST BY CITY
        // ======================================================
        public async Task<WeatherInfoDto?> GetWeatherAsync(string city)
        {
            // Quick coordinate map (Oman + regional cities)
            var coordinates = city.Trim().ToLower() switch
            {
                "muscat" => (lat: 23.5880, lon: 58.3829),
                "salalah" => (lat: 17.0151, lon: 54.0924),
                "sohar" => (lat: 24.3490, lon: 56.7290),
                "nizwa" => (lat: 22.9333, lon: 57.5333),
                _ => (lat: 23.5880, lon: 58.3829) // Default Muscat
            };

            // Build API URL
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={coordinates.lat}&longitude={coordinates.lon}&current_weather=true";

            // Call API
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);
            if (response == null)
            {
                Console.WriteLine($"❌ API returned null for city: {city}");
                return null;
            }


            // Map to WeatherInfoDto
            return new WeatherInfoDto
            {
                Summary = "Current Weather",
                TemperatureC = response.CurrentWeather.Temperature,
                ForecastTimeUtc = DateTime.Parse(response.CurrentWeather.Time)
            };
        }

        // Internal helper model for Open-Meteo response
        private class OpenMeteoResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("current_weather")]
            public CurrentWeatherData? CurrentWeather { get; set; }

            public class CurrentWeatherData
            {
                [System.Text.Json.Serialization.JsonPropertyName("temperature")]
                public double Temperature { get; set; }

                [System.Text.Json.Serialization.JsonPropertyName("windspeed")]
                public double Windspeed { get; set; }

                [System.Text.Json.Serialization.JsonPropertyName("winddirection")]
                public double Winddirection { get; set; }

                [System.Text.Json.Serialization.JsonPropertyName("time")]
                public string Time { get; set; } = string.Empty;
            }

        }


    }
}
