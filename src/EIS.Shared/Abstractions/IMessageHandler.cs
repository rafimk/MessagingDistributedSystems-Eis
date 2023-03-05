using MediatR;

namespace EIS.Shared.Abstractions;

public interface IMessageHandler<in TMessage> where TMessage : class, INotification
{
    Task HandleAsync(TMessage message);
}