using System.Reflection;
using EY.Cqrs.Common;
using Microsoft.Extensions.DependencyInjection;

namespace EY.Cqrs;
public static class CqrsExtension
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<Dispatcher>();

        var assemblyTypes = assembly.GetTypes();
        foreach (var type in assemblyTypes)
        {
            var handlerInterfaces = type.GetInterfaces().Where(Utils.IsHandlerInterface).ToList();
            if (handlerInterfaces.Any())
            {
                var handlerFactory = new HandlerFactory(type);
                foreach (var interfaceType in handlerInterfaces)
                {
                    services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
                }
            }
        }

        return services;
    }

}