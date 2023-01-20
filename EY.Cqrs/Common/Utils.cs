using EY.Cqrs.Common.Commands;
using EY.Cqrs.Common.Queries;

namespace EY.Cqrs.Common;

internal static class Utils
{
    internal static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var typeDefinition = type.GetGenericTypeDefinition();
        return typeDefinition == typeof(ICommandHandler<>) ||
        typeDefinition == typeof(IQueryHandler<,>);
    }
}