using EIS.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EIS.Shared.Dispatchers;

internal sealed class MessageDispatcher : IMessageDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public MessageDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task DispatchAsync<TMessage>(TMessage message) where TMessage : class, INotification
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();
        await handler.HandleAsync(message);
    }
}