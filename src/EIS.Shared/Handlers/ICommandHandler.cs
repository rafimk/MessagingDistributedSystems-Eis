using EIS.Shared.Abstractions;

namespace EIS.Shared.Handlers;

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}