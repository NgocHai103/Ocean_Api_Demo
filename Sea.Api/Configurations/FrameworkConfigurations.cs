using Sea.Infrastructure;

namespace Sea.Api.Configurations;

public static partial class ApiConfigurations
{
    private static IServiceCollection AddFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddInfrastructureServices(configuration);
        return services;
    }

    private static IApplicationBuilder UseFrameworkConfiguration(this IApplicationBuilder builder)
    {
        //builder.ApplyFrameworkMiddlewares();
        return builder;
    }
}
