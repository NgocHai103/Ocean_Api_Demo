using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sea.Application.Handlers;
using Sea.Application.Services;
using Sea.Application.Services.IServices;
using Sea.Share.RabbitMQ;

namespace Sea.Application;
public static class ServicesRegister
{
    public static IServiceCollection AddServicesToContainer(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<IRequestService, RequestService>();
        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.ConfigureRabbitMQ(configuration);

        services.AddScoped<RequestHandler>();

        return services;

    }
}
