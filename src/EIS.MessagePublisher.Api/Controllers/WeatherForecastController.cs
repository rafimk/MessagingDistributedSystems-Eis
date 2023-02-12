using EIS.MessagePublisher.Api.Events;
using EIS.MessagePublisher.Api.Models;
using EIS.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace EIS.MessagePublisher.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public WeatherForecastController(IMessagePublisher messagePublisher, ILogger<WeatherForecastController> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpPost()]
    public async Task<ActionResult> Post()
    {
        var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        foreach (var weatherForecast in weatherForecasts)
        {
            var weatherId = Guid.NewGuid().ToString("N");
            _logger.LogInformation($"Weather with ID: {weatherId} has been placed for summaries : '{weatherForecast.Summary}'.");
            var integrationEvent = new WeatherForecastCreated(
                weatherId,
                weatherForecast.Date,
                weatherForecast.TemperatureC,
                weatherForecast.TemperatureF,
                weatherForecast.Summary);
            await _messagePublisher.PublishAsync("weather-new", integrationEvent);
        }

        return Ok();
    }
}