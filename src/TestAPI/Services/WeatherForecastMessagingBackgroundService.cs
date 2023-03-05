using EIS.Shared.Abstractions;
using EIS.Shared.Messaging;
using TestAPI.Events;

namespace TestAPI.Services;

public sealed class WeatherForecastMessagingBackgroundService: BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly IMessageDispatcher _dispatcher;
    private readonly ILogger<WeatherForecastMessagingBackgroundService> _logger;

    public WeatherForecastMessagingBackgroundService(IMessageSubscriber messageSubscriber, 
        IMessageDispatcher dispatcher,
        ILogger<WeatherForecastMessagingBackgroundService> logger)
    {
        _messageSubscriber = messageSubscriber;
        _dispatcher = dispatcher;
        _logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageSubscriber.SubscribeAsync<WeatherForecastCreated>("weather-new", messageEnvelope =>
        {
            var correlationId = messageEnvelope.CorrelationId;
            _logger.LogInformation($"Weather with ID: '{messageEnvelope.Message.WeatherId}' has been placed. " +
                                   $"Correlation ID: '{correlationId}'.");
            // await _dispatcher.DispatchAsync(msg);
        });
        
        return Task.CompletedTask;
    }
}