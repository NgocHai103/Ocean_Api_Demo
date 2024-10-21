using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Sea.Autofac.IInjectTypes;

namespace Sea.Autofac;

public static class ModuleRegister
{
    public static IServiceCollection AddDependency(this IServiceCollection services)
    {
        //var transientAssembly = typeof(ITransientDependency).Assembly;

        //// Register types using Autofac
        //_ = services.AddAutofac(containerBuilder => containerBuilder.RegisterAssemblyTypes(transientAssembly).AsImplementedInterfaces());
        //builder.Services.AddTransient<IUserService, UserService>();

        services.AddAutofac(containerBuilder =>
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in loadedAssemblies)
            {
                Console.WriteLine($"Loaded Assembly: {assembly.GetName().Name}");
            }

            var myAssembly = typeof(ITransientDependency).Assembly;

            containerBuilder.RegisterAssemblyTypes(myAssembly)
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerDependency();
        });

        return services;
    }
}