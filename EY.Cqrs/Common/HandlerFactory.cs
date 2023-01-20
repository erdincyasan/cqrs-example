using System.Reflection;

namespace EY.Cqrs.Common;

public class HandlerFactory
{
    private readonly List<Func<object, Type, IServiceProvider, object>> _handlerFactoriesPipeline = new List<Func<object, Type, IServiceProvider, object>>();



    public object Create(IServiceProvider provider, Type handlerInterfaceType)
    {
        object currentHandler = null!;
        foreach (var handlerFactory in _handlerFactoriesPipeline)
        {
            currentHandler = handlerFactory(currentHandler, handlerInterfaceType, provider);
        }
        return currentHandler;

    }

    public HandlerFactory(Type type)
    {
        AddHandlerFactory(type);
    }
    private void AddHandlerFactory(Type handlerType, object? attribute = null)
    {
        _handlerFactoriesPipeline.Add(CreateHandler);
        object CreateHandler(object decoratingHandler, Type interfaceType, IServiceProvider serviceprovider)
        {
            ConstructorInfo ctor = MakeGenericTypeSafe(handlerType, interfaceType.GenericTypeArguments).GetConstructors().Single();
            var parameterInfos = ctor.GetParameters();
            var parameters = GetParameters(parameterInfos, decoratingHandler, attribute, serviceprovider);
            var handler = ctor.Invoke(parameters);
            return handler;
        }
    }


    private static Type MakeGenericTypeSafe(Type type, params Type[] typesArgs)
    {
        return type.IsGenericType && !type.GenericTypeArguments.Any() ? type.MakeGenericType(typesArgs) : type;
    }

    private static object[] GetParameters(IEnumerable<ParameterInfo> parameterInfos, object current, object? attribute, IServiceProvider provider)
    {
        return parameterInfos.Select(GetParameter).ToArray();
        object GetParameter(ParameterInfo parameterInfo)
        {
            var parameterType = parameterInfo.ParameterType;
            if (Utils.IsHandlerInterface(parameterType))
                return current;
            if (parameterType == attribute?.GetType())
                return attribute;

            var service = provider.GetService(parameterType);
            if (service != null)
                return service;
            throw new ArgumentException($"Type {parameterType} not found!");
        }

    }
}