using EIS.Shared.Abstractions;
using EIS.Shared.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace EIS.Shared.Dispatchers;

internal sealed class InMemoryQueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public InMemoryQueryDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        if (query is null)
        {
            throw new InvalidOperationException("Query cannot be null.");
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));
        if (method is null)
        {
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");
        }

        return await (Task<TResult>) method.Invoke(handler, new object[] {query, cancellationToken});
    }
}