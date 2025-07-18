namespace MCPfiedSwagger.Example;

public class WeatherForecast
{
    /// <summary>
    /// Represents a weather forecast for a specific date.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the temperature in Celsius.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Summary of the forecast.
    /// </summary>
    public string? Summary { get; set; }
}
