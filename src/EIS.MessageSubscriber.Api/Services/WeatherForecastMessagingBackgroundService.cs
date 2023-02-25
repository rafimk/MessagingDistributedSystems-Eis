using EIS.MessageSubscriber.Api.Events;
using EIS.Shared.Messaging;

namespace EIS.MessageSubscriber.Api.Services;

public sealed class WeatherForecastMessagingBackgroundService: BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly ILogger<WeatherForecastMessagingBackgroundService> _logger;

    public WeatherForecastMessagingBackgroundService(IMessageSubscriber messageSubscriber, 
        ILogger<WeatherForecastMessagingBackgroundService> logger)
    {
        _messageSubscriber = messageSubscriber;
        _logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageSubscriber.SubscribeAsync<WeatherForecastCreated>("weather-new", messageEnvelope =>
        {
            var correlationId = messageEnvelope.CorrelationId;
            _logger.LogInformation($"Weather with ID: '{messageEnvelope.Message.WeatherId}' has been placed. " +
                                   $"Correlation ID: '{correlationId}'.");
        });
        
        return Task.CompletedTask;
    }
}