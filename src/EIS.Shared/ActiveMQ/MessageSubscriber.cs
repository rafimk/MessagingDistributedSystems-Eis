using System.Text.Json;
using ActiveMQ.Artemis.Client;
using EIS.Shared.Accessors;
using EIS.Shared.ActiveMQ.Connection;
using EIS.Shared.Messaging;
using EIS.Shared.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EIS.Shared.ActiveMQ;


public class MessageSubscriber : IMessageSubscriber
{
    private readonly IMessageIdAccessor _messageIdAccessor;
    private readonly IConnection _channel;
    private readonly ILogger<MessageSubscriber> _logger;
    private readonly ISerializer _serializer;

    public MessageSubscriber(IActiveMqChannelFactory activeMqChannelFactory, IMessageIdAccessor messageIdAccessor, ILogger<MessageSubscriber> logger, ISerializer serializer)
    {
        _channel = activeMqChannelFactory.Create();
        _messageIdAccessor = messageIdAccessor;
        _logger = logger;
        _serializer = serializer;
    }
    public async Task SubscribeAsync<T>(string topic, Action<MessageEnvelope<T>> handler) where T : class, INotification
    {
        var stoppingToken = new CancellationToken();
        var topologyManager = await _channel.CreateTopologyManagerAsync();
        var queue = $"{topic}-queue";
        
        await topologyManager.DeclareQueueAsync(new QueueConfiguration
        {
            Address = topic,
            Name = queue,
            RoutingType = RoutingType.Multicast,
            AutoCreateAddress = true,
            Durable = true,
        });
        await using var consumer = await _channel.CreateConsumerAsync(address: topic, queue: queue);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var msg = await consumer.ReceiveAsync();

                var body = msg.GetBody<string>();
                
                var payload = _serializer.Deserialize<T>(body);
                if (payload is not null)
                {
                    _messageIdAccessor.SetMessageId(msg.MessageId);
                    handler(new MessageEnvelope<T>(payload, _messageIdAccessor.GetMessageId()));
                }

                await consumer.AcceptAsync(msg);
 }
            catch (Exception ex)
            {
                // We don't want the background service to stop while the application continues,
                // so catching and logging.
                // Should certainly have some extra checks for the reasons, to act on it. 
                _logger.LogWarning(ex, "Unexpected error while publishing pending outbox messages.");
            }
        }
    }
}