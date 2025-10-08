namespace EventManagementAPI.DTOs.External
{
    /// <summary>
    /// Represents simplified weather forecast data used in event reports.
    /// </summary>
    public class WeatherInfoDto
    {
        // ======================================================
        // 🔹 WEATHER DATA
        // ======================================================
        public string Summary { get; set; } = default!;
        public double? TemperatureC { get; set; }
        public DateTime? ForecastTimeUtc { get; set; }

        public WeatherInfoDto() { }

        public WeatherInfoDto(string summary, double? temperatureC, DateTime? forecastTimeUtc)
        {
            Summary = summary;
            TemperatureC = temperatureC;
            ForecastTimeUtc = forecastTimeUtc;
        }
    }
}
