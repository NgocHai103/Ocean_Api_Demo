using Microsoft.Extensions.DependencyInjection;
using Sea.Domain.Extensions.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sea.Domain.Extensions.AutoDependency;
public static class RegisterToContainer
{
    public static IServiceCollection AddToContainer(this IServiceCollection services, IEnumerable<TypeInfo>? injectTypes)
    {
        if (injectTypes == null)
        {
            return services;
        }

        var dependencyTypes = new Type[] { typeof(ITransientDependency), typeof(IScopeDependency) };

        foreach (var dependencyType in dependencyTypes)
        {
            var injectTypesByType = dependencyType switch
            {
                _ when dependencyType == typeof(ITransientDependency) => injectTypes
                    .Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract),

                _ when dependencyType == typeof(IScopeDependency) => injectTypes
                    .Where(t => typeof(IScopeDependency).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract),

                _ => Enumerable.Empty<TypeInfo>()
            };

            foreach (var injectType in injectTypesByType)
            {
                RegisterInterfaces(services, injectType, dependencyType);
            }
        }

        return services;
    }

    private static void RegisterInterfaces(IServiceCollection services, TypeInfo injectType, Type dependencyType)
    {
        var interfaces = injectType.ImplementedInterfaces
            .Where(x => x.Name != typeof(ITransientDependency).Name);

        foreach (var iface in interfaces)
        {
            // Register for generic interfaces (e.g., IRepository<T>, IService<T>)
            if (iface.IsGenericType)
            {
                RegisterGenericInterface(services, injectType, iface, dependencyType);
                continue;
            }

            // Register the implementation for the non-generic interfaces
            switch (dependencyType)
            {
                case Type _ when typeof(ITransientDependency).IsAssignableFrom(dependencyType):
                    _ = services.AddTransient(iface, injectType);
                    break;

                case Type _ when typeof(IScopeDependency).IsAssignableFrom(dependencyType):
                    _ = services.AddScoped(iface, injectType);
                    break;

                default:
                    break;
            }


        }
    }

    private static void RegisterGenericInterface(IServiceCollection services, TypeInfo injectType, Type iface, Type dependencyType)
    {
        if (iface.IsGenericType)
        {
            var genericTypeDefinition = iface.GetGenericTypeDefinition();

            // Get the concrete implementation type for registration
            var genericArguments = iface.GenericTypeArguments;

            // Check if there's exactly one generic argument
            if (genericArguments.Length == 1)
            {
                var closedGenericType = injectType.MakeGenericType(genericArguments);
                RegisterGeneric(services, closedGenericType, iface, dependencyType);
            }
        }
    }

    private static void RegisterGeneric(IServiceCollection services, Type closedGenericType, Type iface, Type dependencyType)
    {
        switch (dependencyType)
        {
            case Type _ when typeof(ITransientDependency).IsAssignableFrom(dependencyType):
                _ = services.AddTransient(iface, closedGenericType);
                break;

            case Type _ when typeof(IScopeDependency).IsAssignableFrom(dependencyType):
                _ = services.AddScoped(iface, closedGenericType);
                break;
        }
    }
}
