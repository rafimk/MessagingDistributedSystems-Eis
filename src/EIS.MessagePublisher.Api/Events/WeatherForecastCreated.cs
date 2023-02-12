﻿using EIS.Shared.Messaging;

namespace EIS.MessagePublisher.Api.Events;

internal record WeatherForecastCreated(string WeatherId, DateTime Date, int TemperatureC, int TemperatureF, string? Summary) : IMessage;