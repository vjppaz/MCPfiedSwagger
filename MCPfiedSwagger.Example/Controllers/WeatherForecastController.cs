using Microsoft.AspNetCore.Mvc;

namespace MCPfiedSwagger.Example.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static Dictionary<DateOnly, WeatherForecast> _weatherForecasts = new Dictionary<DateOnly, WeatherForecast>();

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get the latest weather forecast
    /// </summary>
    /// <param name="range">Number of days to forecast, default is 5</param>
    /// <returns>The weather forecast for the specified upcoming days.</returns>
    [HttpGet("/WeatherForecast")]
    public IEnumerable<WeatherForecast> Get(int? range = 5)
    {
        var days = Enumerable.Range(1, range.Value);
        return days.Select(m => GetWeatherForecast(DateOnly.FromDateTime(DateTime.Now.AddDays(m))));
    }

    /// <summary>
    /// Get the weather forecast of an specific date
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>The weather forecast of the specified date.</returns>
    [HttpGet("/WeatherForecastByDate")]
    public WeatherForecast GetByDay(DateOnly date)
    {
        return GetWeatherForecast(date);
    }

    private WeatherForecast GetWeatherForecast(DateOnly date)
    {
        if (!_weatherForecasts.ContainsKey(date)) _weatherForecasts[date] = NewForecast(date);

        return _weatherForecasts[date];
    }

    private WeatherForecast NewForecast(DateOnly date)
    {
        return new WeatherForecast
        {
            Date = date,
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };
    }

}
