using System.Reflection;
using EIS.Shared.Messaging;
using EIS.Shared.RabbitMQ.Connections;
using EIS.Shared.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EIS.Shared.RabbitMQ;

public class MessagePublisher : IMessagePublisher
{ 
    private readonly ISerializer _serializer;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<MessagePublisher> _logger;
    private readonly IModel _channel;
    private readonly string _producerName;

    public MessagePublisher(IRabbitMqChannelFactory rabbitMqChannelFactory, 
        ISerializer serializer, IHttpContextAccessor contextAccessor,
        ILogger<MessagePublisher> logger)
    {
        _channel = rabbitMqChannelFactory.Create();
        _serializer = serializer;
        _contextAccessor = contextAccessor;
        _logger = logger;
        _producerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? string.Empty;
        
    }
    public async Task PublishAsync<T>(string topic, T message) where T : class, INotification
    {
        var payload = _serializer.SerializeBytes(message);
        var routingKey = _producerName;

        var properties = _channel.CreateBasicProperties();
        properties.MessageId = Guid.NewGuid().ToString("N");

        _channel.ExchangeDeclare(topic, "topic", false, false);
        _channel.BasicPublish(topic, routingKey, mandatory: true, properties, payload);
        await Task.CompletedTask;
    }
}