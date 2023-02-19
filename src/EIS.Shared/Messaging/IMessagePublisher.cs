using MediatR;

namespace EIS.Shared.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string topic, T message) where T : class, INotification;
}