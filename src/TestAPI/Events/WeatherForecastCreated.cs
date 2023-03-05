using MediatR;

namespace TestAPI.Events;

internal record WeatherForecastCreated(string WeatherId, DateTime Date, int TemperatureC, int TemperatureF, string? Summary) : INotification;