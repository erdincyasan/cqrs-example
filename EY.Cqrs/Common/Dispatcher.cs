using EY.Cqrs.Common.Commands;
using EY.Cqrs.Common.Queries;

namespace EY.Cqrs.Common;

public class Dispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        Type type = typeof(ICommandHandler<>);
        Type[] typeArgs = { command.GetType() };
        Type handlerType = type.MakeGenericType(typeArgs);
        dynamic? handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new ArgumentNullException($"{command.GetType().FullName}'s handler is missing!");
        await handler.HandleAsync((dynamic)command, cancellationToken);
    }

    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
    {
        Type type = typeof(IQueryHandler<,>);
        Type[] typeArgs = { query.GetType(), typeof(TResult) };
        Type handlerType = type.MakeGenericType(typeArgs);
        dynamic? handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new ArgumentNullException(handler);
        TResult result = await handler.HandleAsync((dynamic)query, cancellationToken);
        return result;
    }
}