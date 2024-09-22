using MinimalAPI.Interfaces;
using MinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ISayHello, SayHello>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var weatherGroup = app.MapGroup("/Weather");
var othersGroup = app.MapGroup("/Others");

weatherGroup.MapGet("/weatherforecast", WeatherForecastProcess)
.WithName("GetWeatherForecast")
.WithTags("Weather")
.WithOpenApi();

othersGroup.MapGet("/Teste", (ISayHello sayHello) =>
{
	return Results.Accepted(sayHello.BoasVindas());
})
.WithName("Teste")
.WithTags("Others")
.WithOpenApi();

app.Run();

static IResult WeatherForecastProcess()
{
	var summaries = new[]
	{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

	var forecast = Enumerable.Range(1, 5).Select(index =>
		new WeatherForecast
		(
			DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			Random.Shared.Next(-20, 55),
			summaries[Random.Shared.Next(summaries.Length)]
		))
		.ToArray();

	return TypedResults.Ok(forecast);
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
