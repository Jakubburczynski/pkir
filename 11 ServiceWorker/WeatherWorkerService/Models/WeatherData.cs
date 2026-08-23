namespace WeatherWorkerService.Models;

public class WeatherData
{
    public string Name { get; set; } = string.Empty;

    public MainData Main { get; set; } = new();

    public List<WeatherDescription> Weather { get; set; } = [];
}

public class MainData
{
    public double Temp { get; set; }

    public double Feels_Like { get; set; }

    public int Humidity { get; set; }

    public double Pressure { get; set; }
}

public class WeatherDescription
{
    public string Main { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}