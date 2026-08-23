using System.Text.Json;
using WeatherApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/weather/{city}", async (string city, IHttpClientFactory httpClientFactory) =>
{
    string apiKey = "6e6ab709f13f124514461b6db39b1e15";

    string url =
        $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=pl";

    var client = httpClientFactory.CreateClient();

    var response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        return Results.NotFound("Nie znaleziono miasta.");
    }

    var json = await response.Content.ReadAsStringAsync();

    var weather = JsonSerializer.Deserialize<WeatherResponse>(json);

    return Results.Ok(weather);
});

app.Run();