using EIS.Shared.Messaging;
using MediatR;

namespace EIS.MessageSubscriber.Api.Events;

internal record WeatherForecastCreated(string WeatherId, DateTime Date, int TemperatureC, int TemperatureF, string? Summary) : INotification;