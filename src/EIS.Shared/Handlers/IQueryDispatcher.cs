using EIS.Shared.Abstractions;

namespace EIS.Shared.Handlers;

public interface IQueryDispatcher
{
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}