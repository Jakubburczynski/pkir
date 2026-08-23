using WeatherWorkerService.Models;

namespace WeatherWorkerService.Mappers;

public static class WeatherMapper
{
    public static Weather ToWeather(this WeatherData weatherData)
    {
        return new Weather
        {
            City = weatherData.Name,
            Temperature = weatherData.Main.Temp,
            FeelsLike = weatherData.Main.Feels_Like,
            Humidity = weatherData.Main.Humidity,
            Pressure = weatherData.Main.Pressure,
            Description = weatherData.Weather.FirstOrDefault()?.Description 
                          ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };
    }
}