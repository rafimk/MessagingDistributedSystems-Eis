using MediatR;

namespace EIS.Shared.Abstractions;

public interface IMessageDispatcher
{
    Task DispatchAsync<TMessage>(TMessage message) where TMessage : class, INotification;
}