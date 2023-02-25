using System.Text;
using System.Text.Json;
using EIS.Shared.Accessors;
using EIS.Shared.Messaging;
using EIS.Shared.RabbitMQ.Connections;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EIS.Shared.RabbitMQ;

public class MessageSubscriber : IMessageSubscriber
{
    private readonly IMessageIdAccessor _messageIdAccessor;
    private readonly IModel _channel;

    public MessageSubscriber(IRabbitMqChannelFactory rabbitMqChannelFactory, IMessageIdAccessor messageIdAccessor)
    {
        _channel = rabbitMqChannelFactory.Create();
        _messageIdAccessor = messageIdAccessor;
    }
    public async Task SubscribeAsync<T>(string topic, Action<MessageEnvelope<T>> handler) where T : class, INotification
    {
        var queue = $"{topic}-queue";
        var routingKey = "#";
        _channel.ExchangeDeclare(topic, "topic", durable: false, autoDelete: false, null);
        _channel.QueueDeclare(queue, durable: false, autoDelete: false, exclusive: false);
        _channel.QueueBind(queue, topic, routingKey);
        
        _channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            
            var payload = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

            _messageIdAccessor.SetMessageId(ea.BasicProperties.MessageId);
            
            handler(new MessageEnvelope<T>(payload, _messageIdAccessor.GetMessageId()));
            
            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue, autoAck: false, consumer: consumer);
    }
}