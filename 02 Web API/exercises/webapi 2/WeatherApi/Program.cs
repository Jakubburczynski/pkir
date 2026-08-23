var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

Random random = new Random();

string[] windDirections =
{
    "Północ",
    "Północny-wschód",
    "Wschód",
    "Południowy-wschód",
    "Południe",
    "Południowy-zachód",
    "Zachód",
    "Północny-zachód"
};

app.MapGet("/temperature", () =>
{
    int temperature = random.Next(-20, 41);
    return Results.Ok(temperature);
});

app.MapGet("/wind", () =>
{
    int index = random.Next(windDirections.Length);
    return Results.Ok(windDirections[index]);
});

app.Run();