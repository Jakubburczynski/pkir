namespace WeatherApi.Models;

public class WeatherResponse
{
    public MainData Main { get; set; } = new();
    public WindData Wind { get; set; } = new();
    public List<WeatherData> Weather { get; set; } = new();
    public string Name { get; set; } = "";
}

public class MainData
{
    public double Temp { get; set; }
    public double Feels_Like { get; set; }
    public int Humidity { get; set; }
}

public class WindData
{
    public double Speed { get; set; }
}

public class WeatherData
{
    public string Main { get; set; } = "";
    public string Description { get; set; } = "";
}