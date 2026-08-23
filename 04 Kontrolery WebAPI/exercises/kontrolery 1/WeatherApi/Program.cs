using System.Text.Json;
using WeatherApi.Models;
using WeatherApi.Classes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

List<City> cities = new List<City>();
int nextId = 1;

app.MapPost("/cities", (City city) =>
{
    city.Id = nextId++;
    cities.Add(city);

    return Results.Created($"/cities/{city.Id}", city);
});

app.MapGet("/cities", () =>
{
    return Results.Ok(cities);
});

app.MapGet("/cities/{id}", (int id) =>
{
    var city = cities.FirstOrDefault(c => c.Id == id);

    if (city == null)
    {
        return Results.NotFound("Nie znaleziono miasta.");
    }

    return Results.Ok(city);
});

app.MapPut("/cities/{id}", (int id, City updatedCity) =>
{
    var city = cities.FirstOrDefault(c => c.Id == id);

    if (city == null)
    {
        return Results.NotFound("Nie znaleziono miasta.");
    }

    city.Name = updatedCity.Name;

    return Results.Ok(city);
});

app.MapDelete("/cities/{id}", (int id) =>
{
    var city = cities.FirstOrDefault(c => c.Id == id);

    if (city == null)
    {
        return Results.NotFound("Nie znaleziono miasta.");
    }

    cities.Remove(city);

    return Results.Ok($"Usunięto miasto: {city.Name}");
});

app.MapGet("/weather/{city}", async (string city, IHttpClientFactory httpClientFactory) =>
{
    string apiKey = "6e6ab709f13f124514461b6db39b1e15";

    string url =
        $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=pl";

    var client = httpClientFactory.CreateClient();

    var response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        var error = await response.Content.ReadAsStringAsync();

        return Results.BadRequest(new
        {
            StatusCode = (int)response.StatusCode,
            Error = error
        });
    }

    var json = await response.Content.ReadAsStringAsync();

    var weather = JsonSerializer.Deserialize<WeatherResponse>(json);

    return Results.Ok(weather);
});

app.Run();