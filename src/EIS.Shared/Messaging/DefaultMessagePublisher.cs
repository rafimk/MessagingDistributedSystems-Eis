using MediatR;

namespace EIS.Shared.Messaging;

internal sealed class DefaultMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string topic, T message) where T : class, INotification => Task.CompletedTask;
}