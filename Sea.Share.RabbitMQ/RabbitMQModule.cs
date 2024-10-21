using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sea.Share.RabbitMQ;

public static class RabbitMQModule
{
    public static void ConfigureRabbitMQ(this IServiceCollection services, IConfiguration configuration) => services.ConfigureCap(configuration);

    public static void ConfigureCap(this IServiceCollection services, IConfiguration configuration)
        => _ = services.AddCap(options
            => _ = options.UseSqlServer(configuration["RabbitMQ:DatabaseConnection"] ?? "")
                   .UseRabbitMQ(o =>
                   {
                       o.HostName = configuration["RabbitMQ:Url"] ?? "";
                       o.UserName = configuration["RabbitMQ:Username"] ?? "";
                       o.Password = configuration["RabbitMQ:Password"] ?? "";

                   })
                   .UseDashboard());
}