using System.Buffers;
using System.Collections.Concurrent;
using System.Reflection;
using DotPulsar.Abstractions;
using EIS.Shared.Messaging;
using EIS.Shared.Pulsar;
using EIS.Shared.RabbitMQ.Connections;
using EIS.Shared.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using IMessage = EIS.Shared.Messaging.IMessage;

namespace EIS.Shared.RabbitMQ;

public class RabbitMqMessagePublisher : IMessagePublisher
{ 
    private readonly ISerializer _serializer;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<RabbitMqMessagePublisher> _logger;
    private readonly IModel _channel;
    private readonly string _producerName;

    public RabbitMqMessagePublisher(IChannelFactory channelFactory, 
        ISerializer serializer, IHttpContextAccessor contextAccessor,
        ILogger<RabbitMqMessagePublisher> logger)
    {
        _channel = channelFactory.Create();
        _serializer = serializer;
        _contextAccessor = contextAccessor;
        _logger = logger;
        _producerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? string.Empty;
        
    }
    public async Task PublishAsync<T>(string topic, T message) where T : class, IMessage
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