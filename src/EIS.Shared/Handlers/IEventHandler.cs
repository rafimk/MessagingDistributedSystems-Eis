using EIS.Shared.Abstractions;

namespace EIS.Shared.Handlers;

public interface IEventHandler<in TEvent> where TEvent : class, IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}