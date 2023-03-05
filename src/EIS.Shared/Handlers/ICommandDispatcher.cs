using EIS.Shared.Abstractions;

namespace EIS.Shared.Handlers;

public interface ICommandDispatcher
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : class, ICommand;
}