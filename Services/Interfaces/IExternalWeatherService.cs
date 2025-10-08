using EventManagementAPI.DTOs.External;

namespace EventManagementAPI.Services.Interfaces
{
    public interface IExternalWeatherService
    {
        Task<WeatherInfoDto?> GetWeatherAsync(string city);
    }
}
