using System.Reflection;
using ActiveMQ.Artemis.Client;
using EIS.Shared.ActiveMQ.Connection;
using EIS.Shared.Messaging;
using EIS.Shared.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EIS.Shared.ActiveMQ;

public class MessagePublisher : IMessagePublisher
{ 
    private readonly ISerializer _serializer;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<MessagePublisher> _logger;
    private readonly IConnection _channel;
    private readonly string _producerName;

    public MessagePublisher(IActiveMqChannelFactory activeMqChannelFactory, 
        ISerializer serializer, IHttpContextAccessor contextAccessor,
        ILogger<MessagePublisher> logger)
    {
        _channel = activeMqChannelFactory.Create();
        _serializer = serializer;
        _contextAccessor = contextAccessor;
        _logger = logger;
        _producerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? string.Empty;
        
    }
    public async Task PublishAsync<T>(string topic, T message) where T : class, INotification
    {
        var payload = _serializer.Serialize(message);
        var routingKey = _producerName;

        await using var producer = await _channel.CreateProducerAsync(topic, RoutingType.Multicast);
        var msg = new Message(payload);
        msg.MessageId = Guid.NewGuid().ToString("N");
        
        await producer.SendAsync(msg);

        await Task.CompletedTask;
    }
}