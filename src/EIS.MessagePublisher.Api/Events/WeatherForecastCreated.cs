using EIS.Shared.Messaging;
using MediatR;

namespace EIS.MessagePublisher.Api.Events;

internal record WeatherForecastCreated(string WeatherId, DateTime Date, int TemperatureC, int TemperatureF, string? Summary) : INotification;